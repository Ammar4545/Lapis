
using Lapis_Models;
using Lapis_Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lapis_Utility;
using System.Threading.Tasks;
using Lapis_DataAcess;
using Lapis_DataAcess.Repository.IRepository;

namespace Lapis.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;


        public HomeController(ILogger<HomeController> logger , ApplicationDbContext context, IProductRepository productRepo, ICategoryRepository categoryRepo)
        {
            _logger = logger;
            _context = context;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public IActionResult Index()
        {
            HomeVM home = new HomeVM()
            {
                Products = _productRepo.GetAll(includeProperties: "Category,ApplicationType"),
                categories = _categoryRepo.GetAll()
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
                product = _productRepo.FirstOrDefault(x=>x.Id == id , includeProperties: "Category,ApplicationType"),
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
