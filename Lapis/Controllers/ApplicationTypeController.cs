
using Lapis_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Lapis_Utility;
using Lapis_DataAcess;
using Lapis_DataAcess.Repository;
using Lapis_DataAcess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Lapis.Controllers
{
    [Authorize(Roles =GlobalConst.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly IApplicationTypeRepository _appTypeRepo;

        public ApplicationTypeController(IApplicationTypeRepository appTypeRepo)
        {
            _appTypeRepo = appTypeRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> result = _appTypeRepo.GetAll();

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
            _appTypeRepo.Add(applicationType);
            _appTypeRepo.Save();
            return RedirectToAction("Index");
        }

        // get categories for u then u can Edit one of them 
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var result = _appTypeRepo.Find(id);

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
                _appTypeRepo.Update(applicationType);
                _appTypeRepo.Save();
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

            var result = _appTypeRepo.Find(id);

            if (result is null)
            {
                return NotFound();
            }

            return View(result);
        }

        public IActionResult DeletePost(int? id)
        {
            var category = _appTypeRepo.Find(id);

            if (category == null)
            { return NotFound(); }

            _appTypeRepo.Remove(category);
            _appTypeRepo.Save();
            return RedirectToAction("Index");
        }
    }
}
