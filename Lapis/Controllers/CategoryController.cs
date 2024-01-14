using Lapis.Data;
using Lapis.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace Lapis.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> result = _context.Categories;

            return View(result);
        }
        // get categories for u then u can add 
        public IActionResult Create()
        {
       
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }

        // get categories for u then u can Edit one of them 
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var result = _context.Categories.Find(id);

            if (result is null)
            {
                return NotFound();
            }
            
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }

        // get category object for u then u can delete  
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var result = _context.Categories.Find(id);

            if (result is null)
            {
                return NotFound();
            }

            return View(result);
        }

        public IActionResult DeletePost(int? id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            { return NotFound(); }
            
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
