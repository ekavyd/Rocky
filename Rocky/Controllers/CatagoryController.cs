using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class CatagoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CatagoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Catagory> objList = _db.Catagory;
            return View(objList);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Catagory obj)
        {
            if (ModelState.IsValid)
            {
                _db.Catagory.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        //GET - Edit
        public IActionResult Edit(int id)
        {
            if (null == id || 0 == id)
                return NotFound();

            var data = _db.Catagory.Find(id);

            if(data ==null)
                return NotFound();
            return View(data);
        }

        //POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Catagory obj)
        {
            if (ModelState.IsValid)
            {
                _db.Catagory.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        //GET - Delete
        public IActionResult Delete(int? id)
        {
            if (null == id || 0 == id)
                return NotFound();

            var data = _db.Catagory.Find(id);

            if (data == null)
                return NotFound();
            return View(data);
        }

        //POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var data = _db.Catagory.Find(id);

            if (data !=null)
            {
                _db.Catagory.Remove(data);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return NotFound();
        }



    }
}
