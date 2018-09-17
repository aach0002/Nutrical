using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Assignment_28263103.Models;
using Microsoft.AspNet.Identity;

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
        public ActionResult Create([Bind(Include = "Id,UserId,CaloriesBurnt1,Date")] CaloriesBurnt caloriesBurnt)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con1 = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Authentication.mdf;Integrated Security=True");
                DataTable dt = new DataTable();
                SqlCommand myCommand = new SqlCommand("SELECT TOP 1 Id FROM CaloriesBurnt ORDER BY Id DESC", con1);
                con1.Open();
                SqlDataAdapter da = new SqlDataAdapter(myCommand);
                ////replace with userid from email id
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    caloriesBurnt.Id = Convert.ToInt32(dt.Rows[0]["id"]) + 1;
                }
                caloriesBurnt.UserId = User.Identity.GetUserId();
                //}
                //userDetail.UserId = dt.Rows[0].Id;
                con1.Close();
                da.Dispose();

                db.CaloriesBurnts.Add(caloriesBurnt);
                db.SaveChanges();
                return Redirect("../CaloriesEatens/Index");
                //return RedirectToAction("Index");
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
        public ActionResult Edit([Bind(Include = "Id,UserId,CaloriesBurnt1,Date")] CaloriesBurnt caloriesBurnt)
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
