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
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Assignment_28263103.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private Comments db = new Comments();
        private const String API_KEY = "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";

        // GET: Posts
        public ActionResult Index()
        {
            var posts = from s in db.Posts
                           select s;

            posts = posts.Where(s => s.Approved == "true");

            return View(posts.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminIndex()
        {
            SqlConnection con = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Authentication.mdf;Integrated Security=True");
            List<Post> list_A = new List<Post>();
            DataTable dt = new DataTable();
            SqlCommand myCommand = new SqlCommand("SELECT * FROM Posts ", con);
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            da.Fill(dt);
            list_A = (from DataRow dr in dt.Rows
                      select new Post()
                      {
                          id = Convert.ToInt32(dr["id"]),
                          Title = dr["Title"].ToString(),
                          Post1 = dr["Post"].ToString(),
                          Approved = dr["Approved"].ToString().TrimEnd(),
                          UserId = dr["UserId"].ToString()
                      }).ToList();
            con.Close();
            da.Dispose();
            ViewBag.list = list_A.ToArray();

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Approve(int? id , string email)
        {
            int updated;
            SqlConnection con = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Authentication.mdf;Integrated Security=True");
            string updSql = "Update Posts set Approved='true' where id=" + id ;
            con.Open();
            using (var cmd = new SqlCommand(updSql, con))
            {
                updated = cmd.ExecuteNonQuery();
            }
            con.Close();
            TempData["success"] = "Post has been approved and Email has been sent";

            string mailid = "";
            string title = "";
            string content = "";
            SqlConnection con1 = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Authentication.mdf;Integrated Security=True");
            DataTable dt = new DataTable();
            SqlCommand myCommand = new SqlCommand("Select Email from AspNetUsers where Id='" + email + "'", con1);
            con1.Open();
            SqlDataAdapter da = new SqlDataAdapter(myCommand);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                mailid = Convert.ToString(dt.Rows[0]["Email"]);
            }
            DataTable dt2 = new DataTable();
            SqlCommand myCommand1 = new SqlCommand("Select * from Posts where id = " + id, con1);
            SqlDataAdapter da1 = new SqlDataAdapter(myCommand1);
            da1.Fill(dt2);
            if (dt2.Rows.Count > 0)
            {
                title = Convert.ToString(dt2.Rows[0]["Title"]);
                content = Convert.ToString(dt2.Rows[0]["Post"]);
            }
            con1.Close();
            da.Dispose();

            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("nutricalinfo@gmail.com", "NUTRICAL Post Approval");
            var to = new EmailAddress(mailid, "");
            var plainTextContent = "Congratulations !!! Your post has been approved by the Nutrical Admin. It will be available for all the users to read.";
            var htmlContent = "<p>Hi User,</p><p>" + plainTextContent + "</p><h1>" + title +"</h1><p>" + content + "</p><p>Regards, Nutrical Admin</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, " NUTRICAL Post Approval ", plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
            return Redirect("../adminindex");
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,UserId,Post1,Approved,Date,Title")] Post post)
        {
            if (post.Post1 != null)
            {
                SqlConnection con1 = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Authentication.mdf;Integrated Security=True");
                DataTable dt = new DataTable();
                SqlCommand myCommand = new SqlCommand("SELECT TOP 1 Id FROM Posts ORDER BY Id DESC", con1);
                con1.Open();
                SqlDataAdapter da = new SqlDataAdapter(myCommand);
                ////replace with userid from email id
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    post.id = Convert.ToInt32(dt.Rows[0]["id"]) + 1;
                }
                post.UserId = User.Identity.GetUserId();
                post.Date = DateTime.Now;
                post.Approved = "none";
                //}
                //userDetail.UserId = dt.Rows[0].Id;
                con1.Close();
                da.Dispose();

                db.Posts.Add(post);
                db.SaveChanges();
                TempData["success"] = "Post has been sent for approval";
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,UserId,Post1,Approved,Date")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
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
