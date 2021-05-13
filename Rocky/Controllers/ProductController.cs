using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;  //used to access the wwwroot folder where static images will be stored

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product.Include(p => p.Catagory).Include(p =>p.Application);

            //foreach (var product in objList)
            //{
            //    product.Catagory = _db.Catagory.FirstOrDefault(x => x.Id == product.CatagoryId);
            //    product.Application = _db.Application.FirstOrDefault(x => x.Id == product.ApplicationId);
            //}
            return View(objList);
        }

        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CatagoryDropDown = _db.Catagory.Select(x => new SelectListItem
            //{
            //    Text = x.Name,
            //    Value = x.Id.ToString()
            //});

            //ViewBag.CatagoryDropDown = CatagoryDropDown;
            //ViewData["CatagoryDropDown"] = CatagoryDropDown;

            //var product = new Product();

            var productVm = new ProductVm()
            {
                Product = new Product(),
                CatagorySelectList = _db.Catagory.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                ApplicationSelectList = _db.Application.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (null == id)
            {
                //this is for create
                return View(productVm);
            }
            else 
            {
                productVm.Product = _db.Product.Find(id);
                if (null == productVm.Product)
                    return NotFound();
            }
            return View(productVm);
        }

        //POST - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVm productVm)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVm.Product.Id == 0)
                {
                    //creating new -- get file info
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    //copy file to wwwroot

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVm.Product.Image = fileName + extension;

                    //now add the product to the db with image details saved

                    _db.Product.Add(productVm.Product);

                }
                else 
                {
                    //updating
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(x => x.Id == productVm.Product.Id);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVm.Product.Image = fileName + extension;
                    }
                    else 
                    {
                        productVm.Product.Image = objFromDb.Image;
                    }
                    _db.Product.Update(productVm.Product);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            productVm.CatagorySelectList = _db.Catagory.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            productVm.ApplicationSelectList = _db.Application.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            return View(productVm);
        }

        //GET - Delete
        public IActionResult Delete(int? id)
        {
            if (null == id || 0 == id)
                return NotFound();

            var product = _db.Product.Include(p => p.Catagory).Include(p => p.Application).FirstOrDefault(p => p.Id == id);
            //product.Catagory = _db.Catagory.Find(product.CatagoryId);

            if (product == null)
                return NotFound();
            return View(product);
        }

        //POST - Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var data = _db.Product.Find(id);

            if (data !=null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string upload = webRootPath + WC.ImagePath;

                var oldFile = Path.Combine(upload, data.Image);

                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }

                _db.Product.Remove(data);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return NotFound();
        }



    }
}
