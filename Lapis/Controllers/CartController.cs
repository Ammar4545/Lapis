using Lapis.Config;
using Lapis.Data;
using Lapis.Models;
using Lapis.Models.ViewModel;
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

namespace Lapis.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        public ProductUserVM ProductUserVM { get; set; }
        public CartController(ApplicationDbContext context,IWebHostEnvironment webHostEnvironment , IEmailSender emailSender)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
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
            List<Product> productList = _context.Products.Where(a => shoppingCartProducstsIds.Contains(a.Id)).ToList();
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
            List<Product> productList = _context.Products.Where(a => shoppingCartProducstsIds.Contains(a.Id)).ToList();

            ProductUserVM = new ProductUserVM()
            {
                applicationUser = _context.ApplicationUsers.FirstOrDefault(a => a.Id == claim.Value),
                ProductList = productList
            };
            return View(ProductUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVM ProductUserVM)
        {
            var pathTemplate = _webHostEnvironment.WebRootPath + "/" + "temp" + "/" + "Inquiry.html";

            var subject = "MAIL";
            var htmlBody = "";
            using (StreamReader reader = System.IO.File.OpenText(pathTemplate))
            {
                htmlBody = reader.ReadToEnd();
            }

            StringBuilder productListSB = new StringBuilder();
            foreach (var prod in ProductUserVM.ProductList)
            {
                productListSB.Append($" - Name: { prod.Name} <span style='font-size:14px;'> (ID: {prod.Id})</span><br />");
            }

            string messageBody = string.Format(htmlBody,
                ProductUserVM.applicationUser.FullName,
                ProductUserVM.applicationUser.Email,
                ProductUserVM.applicationUser.PhoneNumber,
                productListSB.ToString());


            await _emailSender.SendEmailAsync(GlobalConst.EmailAdmin, subject, messageBody);

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
