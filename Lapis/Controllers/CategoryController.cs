
using Lapis_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using Lapis_Utility;
using Lapis_DataAcess;
using Lapis_DataAcess.Repository.IRepository;

namespace Lapis.Controllers
{
    [Authorize(Roles = GlobalConst.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryReposiroty;

        public CategoryController(ICategoryRepository categoryReposiroty)
        {
            _categoryReposiroty = categoryReposiroty;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> result = _categoryReposiroty.GetAll();

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
                _categoryReposiroty.Add(category);
                _categoryReposiroty.Save();
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

            var result = _categoryReposiroty.Find(id);

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
                _categoryReposiroty.Update(category);
                _categoryReposiroty.Save();
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

            var result = _categoryReposiroty.Find(id);

            if (result is null)
            {
                return NotFound();
            }

            return View(result);
        }

        public IActionResult DeletePost(int? id)
        {
            var category = _categoryReposiroty.Find(id);

            if (category == null)
            { return NotFound(); }

            _categoryReposiroty.Remove(category);
            _categoryReposiroty.Save();
            return RedirectToAction("Index");
        }


    }
}
