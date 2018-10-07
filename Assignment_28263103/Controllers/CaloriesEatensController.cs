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
    [Authorize]
    public class CaloriesEatensController : Controller
    {
        private Comments db = new Comments();

        // GET: CaloriesEatens
        [Authorize]
        public ActionResult Index(string start, string end , string eaten , string burnt)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                SqlConnection con = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Authentication.mdf;Integrated Security=True");
                string id = User.Identity.GetUserName().ToString();

                List<CaloriesEaten> list_A = new List<CaloriesEaten>();
                List<CaloriesBurnt> list_B = new List<CaloriesBurnt>();

                string datestart = "";
                string dateend = "";
                if (start == null && end == null)
                {
                    DateTime currentDate = DateTime.Now.Date;
                    DateTime dateWeekBefore = currentDate.AddDays(-7);
                    datestart = dateWeekBefore.ToString("yyyy/MM/dd").Substring(0, 10);
                    dateend = currentDate.ToString("yyyy/MM/dd").Substring(0, 10); 
                    //Chart(currentDate.ToString("yyyy/MM/dd").Substring(0, 10), dateWeekBefore.ToString("yyyy/MM/dd").Substring(0, 10));
                }
                else
                {
                    string[] startarray = start.Split('/');
                    datestart = startarray[2] + '-' + startarray[0] + '-' + startarray[1];
                    string[] endarray = end.Split('/');
                    dateend = endarray[2] + '-' + endarray[0] + '-' + endarray[1];
                    //Chart(startd, endd);
                }

                if (eaten == null)
                    eaten = ">0";

                if (burnt == null)
                    burnt = ">0";

                Chart(datestart, dateend , eaten , burnt);

                DataTable dt = new DataTable();
                SqlCommand myCommand = new SqlCommand("SELECT * FROM CaloriesEaten where UserId = '" + userId + "' and Date <= '" + dateend + "' and Date >= '" + datestart + "' and CaloriesEaten " + eaten + " order by Date desc", con);
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
                SqlCommand myCommand2 = new SqlCommand("SELECT * FROM CaloriesBurnt where UserId = '" + userId + "' and Date <= '" + dateend + "' and Date >= '" + datestart + "' and CaloriesBurnt " + burnt + " order by Date desc", con);
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
                
                return View(finalItem);
            }
            catch
            {
                TempData["error"] = "Something wrong happened please try after sometime";
                return null;
            }
        }

        public void Chart(string startDate, string endDate, string eatens, string burnts)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                SqlConnection con = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Authentication.mdf;Integrated Security=True");
                string id = User.Identity.GetUserName().ToString();

                //string[] startarray = startDate.Split('/');
                //string start = startarray[2] + '-' + startarray[0] + '-' + startarray[1];
                //string[] endarray = endDate.Split('/');
                //string end = endarray[2] + '-' + endarray[0] + '-' + endarray[1];

                DataTable dt = new DataTable();
                SqlCommand myCommand = new SqlCommand("SELECT Date,SUM (CaloriesEaten) FROM CaloriesEaten where UserId = '" + userId + "' and Date <= '" + endDate + "' and Date >= '" + startDate + "' and CaloriesEaten " + eatens + " GROUP BY Date order by Date desc", con);
                //SqlCommand myCommand = new SqlCommand("SELECT Date,SUM (CaloriesEaten) FROM CaloriesEaten where UserId = '" + userId + "' GROUP BY Date order by Date desc", con);
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(myCommand);
                da.Fill(dt);
                con.Close();
                da.Dispose();

                DataTable dt2 = new DataTable();
                SqlCommand myCommand2 = new SqlCommand("SELECT Date,SUM (CaloriesBurnt) FROM CaloriesBurnt where UserId = '" + userId + "' and Date <= '" + endDate + "' and Date >= '" + startDate + "' and CaloriesBurnt " + burnts + " GROUP BY Date order by Date desc", con);
                //SqlCommand myCommand2 = new SqlCommand("SELECT Date,SUM (CaloriesBurnt) FROM CaloriesBurnt where UserId = '" + userId + "' GROUP BY Date order by Date desc", con);
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
                        if (Convert.ToDateTime(r1["Date"]) == Convert.ToDateTime(r2["Date"]) && !set)
                        {
                            eaten.Add(r1["Column1"]);
                            burnt.Add(r2["Column1"]);
                            set = true;
                        }
                    }
                    if (!set)
                    {
                        eaten.Add(r1["Column1"]);
                        burnt.Add(0);
                    }
                }

                foreach (DataRow r2 in dt2.Rows)
                {
                    if (labels.Contains(r2["Date"].ToString().Substring(0, 10)) == false)
                    {
                        labels.Add(r2["Date"].ToString().Substring(0, 10));
                        eaten.Add(0);
                        burnt.Add(r2["Column1"]);
                    }
                }

                ViewBag.labels = labels.ToArray();
                ViewBag.eaten = eaten.ToArray();
                ViewBag.burnt = burnt.ToArray();
            }
            catch
            {
                TempData["error"] = "Something wrong happened please try after sometime";
            }
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
                TempData["success"] = "Calories Eaten has been added";
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
            TempData["success"] = "Calories Eaten has been deleted";
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
