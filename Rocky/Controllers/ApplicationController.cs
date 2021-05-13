using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ApplicationController(ApplicationDbContext db)
        {
            _db = db;     
        }
        public IActionResult Index()
        {
            var data = _db.Application;
            return View(data);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Application app)
        {
            _db.Application.Add(app);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET - Edit
        public IActionResult Edit(int id)
        {
            if (null == id || 0 == id)
                return NotFound();

            var data = _db.Application.Find(id);

            if (data == null)
                return NotFound();
            return View(data);
        }

        //POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Application obj)
        {
            if (ModelState.IsValid)
            {
                _db.Application.Update(obj);
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

            var data = _db.Application.Find(id);

            if (data == null)
                return NotFound();
            return View(data);
        }

        //POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var data = _db.Application.Find(id);

            if (data != null)
            {
                _db.Application.Remove(data);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}
