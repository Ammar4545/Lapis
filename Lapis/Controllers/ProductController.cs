
using Lapis_Models;
using Lapis_Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lapis_Utility;
using Lapis_DataAcess;
using Lapis_DataAcess.Repository.IRepository;

namespace Lapis.Controllers
{
    [Authorize(Roles = GlobalConst.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository productRepo  , IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = productRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        public  IActionResult Index()
        {
            IEnumerable<Product> result = _productRepo.GetAll(includeProperties:"Category,ApplicationType");
            //foreach (var item in result)
            //{
            //    item.Category = _productRepo.FirstOrDefault(x => x.Id == item.CategoryId);
            //    item.ApplicationType = _context.ApplicationTypes.FirstOrDefault(x => x.Id == item.ApplicationTypeId);
            //}
            return View(result);
        }
       // get categories for u then u can add

        public IActionResult UpSert(int? id)
        {
            ProductVM productVM = new ProductVM
            {
                Product = new Product(),
                SelectedCategories = _productRepo.GetAllDropDown(GlobalConst.Category),
                SelectedApplicationType = _productRepo.GetAllDropDown(GlobalConst.ApplicationType)
            };
            if (id == null)
            {
                //Creating product it will take u to empty form 
                return View(productVM);
            }
            else
            {
                // we will retrive old context for updateting
                productVM.Product = _productRepo.Find(id.GetValueOrDefault());
                if (productVM.Product is null)
                {
                    return NotFound();
                }
            }
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpSert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var file = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    string uplaod = webRootPath + GlobalConst.ImagePath;
                    string filename = Guid.NewGuid().ToString();
                    string extention = Path.GetExtension(file[0].FileName);

                    using (var filestream = new FileStream(Path.Combine(uplaod, filename + extention), FileMode.Create))
                    {
                        file[0].CopyTo(filestream);
                    }

                    productVM.Product.Image = filename + extention;

                    _productRepo.Add(productVM.Product);
                }
                else
                {
                    var productObjDb = _productRepo.FirstOrDefault(x=>x.Id==productVM.Product.Id,isTracking:false);

                    if (file.Count > 0)
                    {
                        string uplaod = webRootPath + GlobalConst.ImagePath;
                        string filename = Guid.NewGuid().ToString();
                        string extention = Path.GetExtension(file[0].FileName);

                        var oldFileImg = Path.Combine(uplaod + productObjDb.Image);

                        if (System.IO.File.Exists(oldFileImg))
                        {
                            System.IO.File.Delete(oldFileImg);
                        }

                        using (var filestream = new FileStream(Path.Combine(uplaod, filename + extention), FileMode.Create))
                        {
                            file[0].CopyTo(filestream);
                        }
                        productVM.Product.Image = filename + extention;
                    }
                    else
                    {
                        productVM.Product.Image = productObjDb.Image;
                    }
                    _productRepo.Update(productVM.Product);
                }
                _productRepo.Save();
                return RedirectToAction("Index");
            }
            //see if the request is not valid we will go to the view but the dropdwon list will be empty
            //so u should populate it again 

            productVM.SelectedCategories = _productRepo.GetAllDropDown(GlobalConst.Category);
            productVM.SelectedApplicationType = _productRepo.GetAllDropDown(GlobalConst.ApplicationType);

            return View(productVM);
        }

        // get category object for u then u can delete  
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //including category tbl

            var result = _productRepo.FirstOrDefault(x => x.Id == id, includeProperties: "Category,ApplicationType");
            if (result is null)
            {
                return NotFound();
            }
            
            return View(result);
        }
        [HttpPost , ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var productDb = _productRepo.Find(id.GetValueOrDefault());

            if (productDb == null)
            { return NotFound(); }

            string uploadedImages = _webHostEnvironment.WebRootPath + GlobalConst.ImagePath;

            var oldFileImg = Path.Combine(uploadedImages + productDb.Image);

            if (System.IO.File.Exists(oldFileImg))
            {
                System.IO.File.Delete(oldFileImg);
            }


            _productRepo.Remove(productDb);
            _productRepo.Save ();
            return RedirectToAction("Index");
        }


    }
}
