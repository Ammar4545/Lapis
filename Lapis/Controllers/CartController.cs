using Lapis.Config;
using Lapis.Data;
using Lapis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Lapis.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

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
    }
}
