using Lapis.Data;
using Lapis.Models;
using Lapis.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lapis_Utility;
using System.Threading.Tasks;

namespace Lapis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger , ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM home = new HomeVM()
            {
                Products = _context.Products.Include(a => a.Category).Include(a => a.ApplicationType),
                categories = _context.Categories
            };
            return View(home);
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            //check if the session exists
            if (HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey) != null &&
               HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey).Count() > 0)
            {
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey);
            }


            DetailsVM detailsVM = new DetailsVM()
            {
                product = _context.Products.Include(a => a.Category).Include(a => a.ApplicationType)
               .Where(z => z.Id == id).FirstOrDefault(),
                IsExistsInCart = false
            };

            
            foreach (var item in shoppingCarts)
            {
                if (item.ProductId==id)
                {
                    detailsVM.IsExistsInCart = true;
                }
            }

            return View(detailsVM);
        }

        [HttpPost]
        [ActionName(name: "Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            //check if the session exists
            if (HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey) !=null &&
               HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey).Count() >0)
            {
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey);
            }
            shoppingCarts.Add(new ShoppingCart { ProductId = id });
            HttpContext.Session.Set(GlobalConst.CartKey, shoppingCarts);
            return RedirectToAction("Index");
        }

       
        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCarts = new List<ShoppingCart>();
            //check if the session exists
            if (HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey) != null &&
               HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey).Count() > 0)
            {
                shoppingCarts = HttpContext.Session.Get<List<ShoppingCart>>(GlobalConst.CartKey);
            }
            // the item we choice for delete 
            var itemForRemoving = shoppingCarts.SingleOrDefault(a => a.ProductId==id);
            //cheching
            if (itemForRemoving != null)
            {
                shoppingCarts.Remove(itemForRemoving);
            }
            //updaating the list before removing the item
            //add the new list to the session
            HttpContext.Session.Set(GlobalConst.CartKey, shoppingCarts);

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    
    }
    
}
