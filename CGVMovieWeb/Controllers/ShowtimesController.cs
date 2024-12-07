using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CGVMovieWeb.Models;

namespace CGVMovieWeb.Controllers
{
    public class ShowtimesController : Controller
    {
        private CGV_BinhDuong_SquareEntities db = new CGV_BinhDuong_SquareEntities();

        // GET: Showtimes
        public ActionResult Index()
        {
            var showtimes = db.Showtimes.Include(s => s.Movy);
            return View(showtimes.ToList());
        }

        // GET: Showtimes/DatVe/5
        public ActionResult DatVe(int movieId)
        {
            var userId = Session["UserID"];
            if (userId == null)
            {
                // Xử lý khi không có UserID trong Session, ví dụ: chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login", "Account");
            }
            var movie = db.Movies.Find(movieId);
            if (movie == null)
            {
                return HttpNotFound();
            }

            var showtimes = db.Showtimes.Where(s => s.MovieID == movieId).ToList();

            var model = new DatVeViewModel
            {
                Movie = movie,
                Showtimes = showtimes
            };

            return View(model);
        }

        // GET: Showtimes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Showtime showtime = db.Showtimes.Find(id);
            if (showtime == null)
            {
                return HttpNotFound();
            }
            return View(showtime);
        }

        // GET: Showtimes/Create
        public ActionResult Create()
        {
            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "Title");
            return View();
        }

        // POST: Showtimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShowtimeID,MovieID,ShowDate,StartTime,Room")] Showtime showtime)
        {
            if (ModelState.IsValid)
            {
                db.Showtimes.Add(showtime);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "Title", showtime.MovieID);
            return View(showtime);
        }

        // GET: Showtimes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Showtime showtime = db.Showtimes.Find(id);
            if (showtime == null)
            {
                return HttpNotFound();
            }
            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "Title", showtime.MovieID);
            return View(showtime);
        }

        // POST: Showtimes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShowtimeID,MovieID,ShowDate,StartTime,Room")] Showtime showtime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(showtime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "Title", showtime.MovieID);
            return View(showtime);
        }

        // GET: Showtimes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Showtime showtime = db.Showtimes.Find(id);
            if (showtime == null)
            {
                return HttpNotFound();
            }
            return View(showtime);
        }

        // POST: Showtimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Showtime showtime = db.Showtimes.Find(id);
            db.Showtimes.Remove(showtime);
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
