using Lapis.Config;
using Lapis.Data;
using Lapis.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Lapis.Controllers
{
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
    }
}
