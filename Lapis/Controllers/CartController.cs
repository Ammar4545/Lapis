using Lapis.Config;
using Lapis.Data;
using Lapis.Models;
using Lapis.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Lapis.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductUserVM ProductUserVM { get; set; }
        public CartController(ApplicationDbContext context)
        {
            _context = context;
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

        
        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartProducts = new List<ShoppingCart>();

            if (HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey) != null
                && HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey).Count > 0)
            {
                shoppingCartProducts = HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey);
            }

            shoppingCartProducts.Remove(shoppingCartProducts.FirstOrDefault(a => a.ProductId==id));

            HttpContext.Session.Set(GlobalConst.CartKey, shoppingCartProducts);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Post")]
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
    }
}
