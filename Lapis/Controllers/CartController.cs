using Lapis_Utility;
using Lapis_Models;
using Lapis_Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Lapis_DataAcess;
using Lapis_DataAcess.Repository.IRepository;
using System;

namespace Lapis.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        private readonly IProductRepository _productRepo;
        private readonly IApplicationUserRepository _userRepo;
        private readonly IInquiryDetailRepository _inquiryDetailRepo;
        private readonly IInquiryHeaderRepository _inquiryHeaderRepo;

        public ProductUserVM ProductUserVM { get; set; }
        public CartController(ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment, IEmailSender emailSender,
            IProductRepository productRepo, IApplicationUserRepository userRepo,
            IInquiryDetailRepository inquiryDetailRepo, IInquiryHeaderRepository inquiryHeaderRepo) 
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _productRepo = productRepo;
            _userRepo = userRepo;
            _inquiryDetailRepo = inquiryDetailRepo;
            _inquiryHeaderRepo = inquiryHeaderRepo;
        }
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartProducts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey).Count > 0)
            {
                shoppingCartProducts = HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey);
            }
            List<int> shoppingCartProducstsIds = shoppingCartProducts.Select(a => a.ProductId).ToList();
            List<Product> productList = _productRepo.GetAll(a => shoppingCartProducstsIds.Contains(a.Id)).ToList();
            return View(productList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {

            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {

            // represents the identity of a user and contains a collection of claims.
            //It is created during the authentication process and carries information about the user.
            //[ User.Identity] returns IIdentitiy
            //casting (ClaimsIdentity) that aslo implement IIdentity add extra method for u
            var claimIdentity =(ClaimsIdentity) User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            // u can use this code [  var userUd=User.FindFirstValue(ClaimType.Name)  ]

            List<ShoppingCart> shoppingCartProducts = new List<ShoppingCart>();
            if (HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey).Count > 0)
            {
                shoppingCartProducts = HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey);
            }
            List<int> shoppingCartProducstsIds = shoppingCartProducts.Select(a => a.ProductId).ToList();
            List<Product> productList = _productRepo.GetAll(a => shoppingCartProducstsIds.Contains(a.Id)).ToList();

            ProductUserVM = new ProductUserVM()
            {
                applicationUser = _userRepo.FirstOrDefault(a => a.Id == claim.Value),
                ProductList = productList
            };
            return View(ProductUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVM ProductUserVM)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var pathTemplate = _webHostEnvironment.WebRootPath + "/" + "temp" + "/" + "Inquiry.html";

            var subject = "MAIL";
            var htmlBody = "";
            using (StreamReader reader = System.IO.File.OpenText(pathTemplate))
            {
                htmlBody = reader.ReadToEnd();
            }

            StringBuilder productListSB = new StringBuilder();
            foreach (var product in ProductUserVM.ProductList)
            {
                productListSB.Append($" - Name: { product.Name} <span style='font-size:14px;'> (ID: {product.Id})</span><br />");
            }

            string messageBody = string.Format(htmlBody,
                ProductUserVM.applicationUser.FullName,
                ProductUserVM.applicationUser.Email,
                ProductUserVM.applicationUser.PhoneNumber,
                productListSB.ToString());


            await _emailSender.SendEmailAsync(GlobalConst.EmailAdmin, subject, messageBody);
            InquiryHeaders inquiryHeaders = new InquiryHeaders()
            {
                
                ApplicationUser = ProductUserVM.applicationUser,
                ApplicationUserId = claim.Value,
                FullName = ProductUserVM.applicationUser.FullName,
                Email = ProductUserVM.applicationUser.Email,
                PhoneNumber = ProductUserVM.applicationUser.PhoneNumber,
                InquiryDate = DateTime.Now,

            };
            _inquiryHeaderRepo.Add(inquiryHeaders);
            _inquiryHeaderRepo.Save();

            foreach (var product in ProductUserVM.ProductList)
            {
                InquiryDetails inquiryDetails = new InquiryDetails()
                {
                    InquiryHeaderId= inquiryHeaders.Id,
                    ProductId= product.Id,


                };
                _inquiryDetailRepo.Add(inquiryDetails);
            }

            _inquiryDetailRepo.Save();  

            return RedirectToAction(nameof(InquiryConfirm));
        }

        public IActionResult InquiryConfirm()
        {

            HttpContext.Session.Clear();
            return View();
        }


        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartProducts = new List<ShoppingCart>();

            if (HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey).Count > 0)
            {
                shoppingCartProducts = HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey);
            }

            shoppingCartProducts.Remove(shoppingCartProducts.FirstOrDefault(a => a.ProductId == id));

            HttpContext.Session.Set(GlobalConst.CartKey, shoppingCartProducts);

            return RedirectToAction(nameof(Index));
        }
    }
}
