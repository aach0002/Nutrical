using System;
using System.Collections;
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

            SqlConnection con = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Authentication.mdf;Integrated Security=True");
            string id = User.Identity.GetUserName().ToString();

            List<CaloriesEaten> list_A = new List<CaloriesEaten>();
            List<CaloriesBurnt> list_B = new List<CaloriesBurnt>();

            DataTable dt = new DataTable();
            SqlCommand myCommand = new SqlCommand("SELECT * FROM CaloriesEaten ORDER BY Date DESC", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            da.Fill(dt);
            list_A = (from DataRow dr in dt.Rows
                           select new CaloriesEaten()
                           {
                               CaloriesEaten1 = Convert.ToInt32(dr["CaloriesEaten"]),
                               Type = dr["Type"].ToString(),
                               Date = Convert.ToDateTime(dr["Date"]),
                           }).ToList();
            con.Close();
            da.Dispose();

            DataTable dt2 = new DataTable();
            SqlCommand myCommand2 = new SqlCommand("SELECT * FROM CaloriesBurnt ORDER BY Date DESC", con);
            con.Open();
            SqlDataAdapter da2 = new SqlDataAdapter(myCommand2);
            da2.Fill(dt2);
            list_B = (from DataRow dr in dt2.Rows
                      select new CaloriesBurnt()
                      {
                          CaloriesBurnt1 = Convert.ToInt32(dr["CaloriesBurnt"]),
                          Date = Convert.ToDateTime(dr["date"]),
                      }).ToList();
            con.Close();
            da2.Dispose();
            CombinedCalories finalItem = new CombinedCalories();
            finalItem.ListA = list_A;
            finalItem.ListB = list_B;
            Chart();
            return View(finalItem);

            //var userId = User.Identity.GetUserId();

            
        }

        public void Chart()
        {
            SqlConnection con = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Authentication.mdf;Integrated Security=True");
            string id = User.Identity.GetUserName().ToString();

            DataTable dt = new DataTable();
            SqlCommand myCommand = new SqlCommand("SELECT Date,SUM (CaloriesEaten) FROM CaloriesEaten GROUP BY Date;", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            da.Fill(dt);
            con.Close();
            da.Dispose();

            DataTable dt2 = new DataTable();
            SqlCommand myCommand2 = new SqlCommand("SELECT Date,SUM (CaloriesBurnt) FROM CaloriesBurnt GROUP BY Date;", con);
            con.Open();
            SqlDataAdapter da2 = new SqlDataAdapter(myCommand2);
            da2.Fill(dt2);
            con.Close();
            da2.Dispose();

            ArrayList labels = new ArrayList();
            ArrayList eaten = new ArrayList();
            ArrayList burnt = new ArrayList();

            foreach (DataRow r1 in dt.Rows)
            {
                bool set = false;
                labels.Add(r1["Date"].ToString().Substring(0, 10));
                foreach (DataRow r2 in dt2.Rows)
                {
                    if(Convert.ToDateTime(r1["Date"]) == Convert.ToDateTime(r2["Date"]))
                    {
                        eaten.Add(r1["Column1"]);
                        burnt.Add(r2["Column1"]);
                    }
                }
                eaten.Add(r1["Column1"]);
                burnt.Add(0);
            }

            ViewBag.labels = labels.ToArray();
            ViewBag.eaten = eaten.ToArray();
            ViewBag.burnt = burnt.ToArray();

            //ArrayList xValue = new ArrayList();

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

        [Authorize]
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
                SqlCommand myCommand = new SqlCommand("SELECT TOP 1 Id FROM CaloriesEaten ORDER BY Id DESC", con1);
                con1.Open();
                SqlDataAdapter da = new SqlDataAdapter(myCommand);
                ////replace with userid from email id
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    caloriesEaten.Id = Convert.ToInt32(dt.Rows[0]["id"]) + 1;
                }
                    caloriesEaten.UserId = User.Identity.GetUserId();
                //}
                //userDetail.UserId = dt.Rows[0].Id;
                con1.Close();
                da.Dispose();

                db.CaloriesEatens.Add(caloriesEaten);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(caloriesEaten);
        }

        [Authorize]
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

        [Authorize]
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
