﻿using System;
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
    public class CaloriesEatensController : Controller
    {
        private Comments db = new Comments();

        // GET: CaloriesEatens
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            return View(db.CaloriesEatens.ToList());
        }

        // GET: CaloriesEatens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaloriesEaten caloriesEaten = db.CaloriesEatens.Find(id);
            if (caloriesEaten == null)
            {
                return HttpNotFound();
            }
            return View(caloriesEaten);
        }

        // GET: CaloriesEatens/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CaloriesEatens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CaloriesEaten1,Type,Date,UserId")] CaloriesEaten caloriesEaten)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con1 = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Authentication.mdf;Integrated Security=True");
                DataTable dt = new DataTable();
                string id = User.Identity.GetUserName().ToString();
                SqlCommand myCommand = new SqlCommand("Select * from AspNetUsers where Email = '" + id + "'", con1);
                con1.Open();
                SqlDataAdapter da = new SqlDataAdapter(myCommand);
                //replace with userid from email id
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    caloriesEaten.UserId = Convert.ToString(dt.Rows[0]["id"]);
                }
                //userDetail.UserId = dt.Rows[0].Id;
                con1.Close();
                da.Dispose();

                db.Entry(caloriesEaten).State = EntityState.Modified;
                //db.CaloriesEatens.Add(caloriesEaten);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(caloriesEaten);
        }

        // GET: CaloriesEatens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaloriesEaten caloriesEaten = db.CaloriesEatens.Find(id);
            if (caloriesEaten == null)
            {
                return HttpNotFound();
            }
            return View(caloriesEaten);
        }

        // POST: CaloriesEatens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CaloriesEaten1,Type,Date,UserId")] CaloriesEaten caloriesEaten)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caloriesEaten).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(caloriesEaten);
        }

        // GET: CaloriesEatens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaloriesEaten caloriesEaten = db.CaloriesEatens.Find(id);
            if (caloriesEaten == null)
            {
                return HttpNotFound();
            }
            return View(caloriesEaten);
        }

        // POST: CaloriesEatens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CaloriesEaten caloriesEaten = db.CaloriesEatens.Find(id);
            db.CaloriesEatens.Remove(caloriesEaten);
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
