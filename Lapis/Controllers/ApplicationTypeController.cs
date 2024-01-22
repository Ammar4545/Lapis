using Lapis.Data;
using Lapis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lapis_Utility;

namespace Lapis.Controllers
{
    [Authorize(Roles =GlobalConst.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationTypeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> result = _context.ApplicationTypes;

            return View(result);
        }
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType applicationType)
        {
            _context.ApplicationTypes.Add(applicationType);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // get categories for u then u can Edit one of them 
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var result = _context.ApplicationTypes.Find(id);

            if (result is null)
            {
                return NotFound();
            }

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType applicationType)
        {
            if (ModelState.IsValid)
            {
                _context.ApplicationTypes.Update(applicationType);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(applicationType);
            }
        }

        // get category object for u then u can delete  
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var result = _context.ApplicationTypes.Find(id);

            if (result is null)
            {
                return NotFound();
            }

            return View(result);
        }

        public IActionResult DeletePost(int? id)
        {
            var category = _context.ApplicationTypes.Find(id);

            if (category == null)
            { return NotFound(); }

            _context.ApplicationTypes.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
