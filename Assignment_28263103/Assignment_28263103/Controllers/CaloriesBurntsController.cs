using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment_28263103.Models;

namespace Assignment_28263103.Controllers
{
    public class CaloriesBurntsController : Controller
    {
        private Comments db = new Comments();

        // GET: CaloriesBurnts
        public ActionResult Index()
        {
            return View(db.CaloriesBurnts.ToList());
        }

        // GET: CaloriesBurnts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaloriesBurnt caloriesBurnt = db.CaloriesBurnts.Find(id);
            if (caloriesBurnt == null)
            {
                return HttpNotFound();
            }
            return View(caloriesBurnt);
        }

        // GET: CaloriesBurnts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CaloriesBurnts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,CaloriesBurnt1")] CaloriesBurnt caloriesBurnt)
        {
            if (ModelState.IsValid)
            {
                db.CaloriesBurnts.Add(caloriesBurnt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(caloriesBurnt);
        }

        // GET: CaloriesBurnts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaloriesBurnt caloriesBurnt = db.CaloriesBurnts.Find(id);
            if (caloriesBurnt == null)
            {
                return HttpNotFound();
            }
            return View(caloriesBurnt);
        }

        // POST: CaloriesBurnts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,CaloriesBurnt1")] CaloriesBurnt caloriesBurnt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caloriesBurnt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(caloriesBurnt);
        }

        // GET: CaloriesBurnts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaloriesBurnt caloriesBurnt = db.CaloriesBurnts.Find(id);
            if (caloriesBurnt == null)
            {
                return HttpNotFound();
            }
            return View(caloriesBurnt);
        }

        // POST: CaloriesBurnts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CaloriesBurnt caloriesBurnt = db.CaloriesBurnts.Find(id);
            db.CaloriesBurnts.Remove(caloriesBurnt);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
