using CGVMovieWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CGVMovieWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly CGV_BinhDuong_SquareEntities _context; 

        public HomeController()
        {
            _context = new CGV_BinhDuong_SquareEntities(); 
        }
        public ActionResult Index()
        {
            if (Session["Role"] != null && Session["Role"].ToString() == "Staff")
            {
                // Nếu là Staff, chuyển hướng đến phương thức Index của AdminController
                return RedirectToAction("MoviesList", "Admin");
            }

            var userID = Session["UserID"]; // Giả sử ID người dùng lưu trong Session
            if (userID != null)
            {
                var tickets = _context.Tickets.Where(t => t.UserID == (int)userID).ToList();

            ViewBag.movieOpen = TempData.ContainsKey("movieOpen") && TempData["movieOpen"] != null
            ? TempData["movieOpen"]
            : null;



                // Lưu giỏ vé vào session
                Session["Tickets"] = tickets;
            }

            // Lấy danh sách các chủ đề phim
            var genres = _context.Movies.Select(m => m.Genre).Distinct().ToList();

            // Lấy danh sách các bộ phim
            var movies = _context.Movies.ToList();

            // Truyền dữ liệu vào ViewBag
            ViewBag.Genres = genres;

            return View(movies);
        }



        public ActionResult ChiTiet(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.MovieID == id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        [HttpGet]
        public ActionResult Back()
        {
            TempData["movieOpen"] = "active";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LienHe()
        {
            return View();
        }

        public ActionResult DatVe()
        {
            return View();
        }
    }
}