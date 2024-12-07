using CGVMovieWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CGVMovieWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly CGV_BinhDuong_SquareEntities _context;

        public AdminController()
        {
            _context = new CGV_BinhDuong_SquareEntities();
        }

        // 1. Hiển thị danh sách phim
        public ActionResult MoviesList()
        {
            var userID = Session["UserID"];
            if (userID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var movies = _context.Movies
                         .Select(m => new MovieViewModel
                         {
                             MovieID = m.MovieID,
                             Title = m.Title,
                             Genre = m.Genre,
                             Duration = m.Duration,
                             Description = m.Description,
                             ReleaseDate = m.ReleaseDate,
                             Rating = m.Rating,
                             TicketPrice = (decimal)m.TicketPrice,
                             ImageLink = m.ImageLink
                         })
                         .ToList();
            return View(movies);
        }

        // 2. Thêm phim mới
        [HttpGet]
        public ActionResult CreateMovie()
        {
            var userID = Session["UserID"];
            if (userID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(new MovieViewModel());
        }
        [HttpPost]
        public ActionResult CreateMovie(MovieViewModel model, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var movie = new Movy
                {
                    Title = model.Title,
                    Genre = model.Genre,
                    Duration = model.Duration,
                    Description = model.Description,
                    ReleaseDate = model.ReleaseDate,
                    Rating = model.Rating,
                    TicketPrice = model.TicketPrice
                };

                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    ImageFile.SaveAs(path);
                    movie.ImageLink = fileName;
                }

                _context.Movies.Add(movie);
                _context.SaveChanges();
                return RedirectToAction("MoviesList");
            }
            return View(model);
        }
        // GET Edit Movie
        [HttpGet]
        public ActionResult EditMovie(int id)
        {
            var userID = Session["UserID"];
            if (userID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var movie = _context.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            var model = new Movy
            {
                MovieID = movie.MovieID,
                Title = movie.Title,
                Genre = movie.Genre,
                Duration = movie.Duration,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Rating = movie.Rating,
                TicketPrice = movie.TicketPrice,
                ImageLink = movie.ImageLink
            };

            return View(model);
        }

        // POST Edit Movie
        [HttpPost]
        public ActionResult EditMovie(MovieViewModel model, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var movie = _context.Movies.Find(model.MovieID);
                if (movie == null)
                {
                    return HttpNotFound();
                }

                movie.Title = model.Title;
                movie.Genre = model.Genre;
                movie.Duration = model.Duration;
                movie.Description = model.Description;
                movie.ReleaseDate = model.ReleaseDate;
                movie.Rating = model.Rating;
                movie.TicketPrice = model.TicketPrice;

                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    ImageFile.SaveAs(path);
                    movie.ImageLink = fileName;
                }

                _context.Entry(movie).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("MoviesList");
            }
            return View(model);
        }


        // 4. Xóa phim
        [HttpPost]
        public ActionResult DeleteMovie(int id)
        {
            var userID = Session["UserID"];
            if (userID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var movie = _context.Movies.Find(id);

            if (movie != null)
            {
                // Xóa các vé trong bảng Tickets liên quan đến ghế của lịch chiếu
                var showtimes = _context.Showtimes.Where(s => s.MovieID == id).ToList();

                foreach (var showtime in showtimes)
                {
                    var seats = _context.Seats.Where(seat => seat.ShowtimeID == showtime.ShowtimeID).ToList();

                    // Xóa các vé liên quan đến ghế
                    foreach (var seat in seats)
                    {
                        var tickets = _context.Tickets.Where(ticket => ticket.SeatID == seat.SeatID).ToList();
                        _context.Tickets.RemoveRange(tickets);  // Xóa các vé liên quan đến ghế
                    }

                    // Xóa các ghế liên quan đến lịch chiếu
                    _context.Seats.RemoveRange(seats);
                }

                // Xóa các lịch chiếu liên quan đến bộ phim
                _context.Showtimes.RemoveRange(showtimes);
                _context.SaveChanges(); // Lưu thay đổi

                // Xóa bộ phim
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }

            return RedirectToAction("MoviesList");
        }


        // 5. Hiển thị danh sách lịch chiếu
        public ActionResult ShowtimesList()
        {
            var userID = Session["UserID"];
            if (userID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var showtimes = _context.Showtimes
                .Include(s => s.Movy) // Bao gồm thông tin phim
                .Select(s => new ShowtimeViewModel
                {
                    ShowtimeID = s.ShowtimeID,
                    MovieID = s.MovieID ?? 0, // Sử dụng giá trị mặc định nếu MovieID là null
                    ShowDate = s.ShowDate ?? DateTime.Now, // Giá trị mặc định nếu ShowDate là null
                    StartTime = s.StartTime ?? TimeSpan.Zero, // Giá trị mặc định nếu StartTime là null
                    Room = s.Room,
                    MovieTitle = s.Movy.Title // Lấy tiêu đề phim từ Movie
                }).ToList();

            return View(showtimes);
        }

        // Thêm lịch chiếu
        [HttpGet]
        public ActionResult CreateShowtime()
        {
            var userID = Session["UserID"];
            if (userID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.MovieID = new SelectList(_context.Movies, "MovieID", "Title");
            return View();
        }

        [HttpPost]
        public ActionResult CreateShowtime(Showtime showtime)
        {
            if (ModelState.IsValid)
            {
                _context.Showtimes.Add(showtime);
                _context.SaveChanges(); // Lưu lịch chiếu trước để có được ShowTimeID

                // Tạo ghế cho phòng
                string room = showtime.Room;
                List<Seat> seats = new List<Seat>();
                char[] rows = { 'A', 'B', 'C', 'D', 'E', 'F' };
                int seatsPerRow = 8;

                foreach (char row in rows)
                {
                    for (int seatNumber = 1; seatNumber <= seatsPerRow; seatNumber++)
                    {
                        var seat = new Seat
                        {
                            Room = room,
                            SeatNumber = $"{row}{seatNumber}",
                            IsAvailable = true, // Mặc định là có sẵn
                            ShowtimeID = showtime.ShowtimeID
                        };
                        seats.Add(seat);
                    }
                }

                _context.Seats.AddRange(seats); // Thêm danh sách ghế vào cơ sở dữ liệu
                _context.SaveChanges(); // Lưu thay đổi

                return RedirectToAction("ShowtimesList");
            }

            ViewBag.MovieID = new SelectList(_context.Movies, "MovieID", "Title", showtime.MovieID);
            return View(showtime);
        }

        // Sửa lịch chiếu
        [HttpGet]
        public ActionResult EditShowtime(int id)
        {
            var userID = Session["UserID"];
            if (userID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var showtime = _context.Showtimes.Find(id);
            if (showtime == null)
            {
                return HttpNotFound();
            }
            ViewBag.MovieID = new SelectList(_context.Movies, "MovieID", "Title", showtime.MovieID);
            return View(showtime);
        }

        [HttpPost]
        public ActionResult EditShowtime(Showtime showtime)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(showtime).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("ShowtimesList");
            }
            ViewBag.MovieID = new SelectList(_context.Movies, "MovieID", "Title", showtime.MovieID);
            return View(showtime);
        }

        // Xóa lịch chiếu
        [HttpPost]
        public ActionResult DeleteShowtime(int id)
        {
            var showtime = _context.Showtimes.Find(id);
            if (showtime != null)
            {
                _context.Showtimes.Remove(showtime);
                _context.SaveChanges();
            }
            return RedirectToAction("ShowtimesList");
        }


        // 7. Thống kê doanh thu
        public ActionResult RevenueStatistics()
        {
            var userID = Session["UserID"];
            if (userID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            // Truy vấn doanh thu và chi tiết vé bán được cho mỗi ngày
            var revenueData = _context.Tickets
                .Where(t => t.PurchaseDate.HasValue && t.PaymentConfirmation == true) 
                .GroupBy(t => DbFunctions.TruncateTime(t.PurchaseDate.Value)) 
                .Select(g => new
                {
                    Date = g.Key,
                    TotalRevenue = g.Sum(t => t.Price ?? 0), // Tổng doanh thu
                    Tickets = g.Select(t => new TicketViewModel
                    {
                        TicketID = t.TicketID,
                        Username = t.User.FullName,
                        MovieTitle = t.Showtime.Movy.Title,
                        ShowDate = t.Showtime.ShowDate ?? DateTime.Now,
                        StartTime = t.Showtime.StartTime ?? TimeSpan.Zero,
                        Room = t.Showtime.Room,
                        Price = t.Price ?? 0,
                        PurchaseDate = t.PurchaseDate ?? DateTime.Now,
                        SeatNumber = t.Seat.SeatNumber,
                        PaymentConfirmation = t.PaymentConfirmation ?? false,
                        PaymentMethod = t.PaymentMethod 
                    }).ToList()
                })
                .ToList()
                .Select(r => new RevenueStatisticsViewModel
                {
                    Date = r.Date.HasValue ? r.Date.Value.ToString("dd/MM/yyyy") : "N/A", // Chuyển đổi sau khi truy vấn
                    TotalRevenue = r.TotalRevenue,
                    Tickets = r.Tickets
                })
                .ToList();

            return View(revenueData);
        }


        // Hiển thị danh sách vé đã đặt
        public ActionResult TicketsList(string searchTerm = "")
        {
            var userID = Session["UserID"];
            if (userID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var ticketsQuery = _context.Tickets
                .Include(t => t.Showtime)
                .Include(t => t.Showtime.Movy)
                .Include(t => t.User)
                .Include(t => t.Seat)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                ticketsQuery = ticketsQuery.Where(t => t.User.FullName.Contains(searchTerm) ||
                                               t.Showtime.Movy.Title.Contains(searchTerm) ||
                                               t.Showtime.Room.Contains(searchTerm) ||
                                               t.Seat.SeatNumber.Contains(searchTerm) ||
                                               t.TicketID.ToString().Contains(searchTerm));
            }

            var tickets = ticketsQuery.Select(t => new TicketViewModel
            {
                TicketID = t.TicketID,
                Username = t.User.FullName,
                MovieTitle = t.Showtime.Movy.Title,
                ShowDate = t.Showtime.ShowDate ?? DateTime.Now,
                StartTime = t.Showtime.StartTime ?? TimeSpan.Zero,
                Room = t.Showtime.Room,
                Price = t.Price ?? 0,
                PurchaseDate = t.PurchaseDate ?? DateTime.Now,
                SeatNumber = t.Seat.SeatNumber,
                PaymentConfirmation = t.PaymentConfirmation ?? false,
                PaymentMethod = t.PaymentMethod ?? "Chưa Thanh Toán"
            }).ToList();

            return View(tickets);
        }



        [HttpGet]
        public ActionResult EditTicket(int id)
        {
            var ticket = _context.Tickets
                .Include(t => t.Showtime)
                .Include(t => t.User)
                .Include(t => t.Seat) // Bao gồm thông tin ghế
                .FirstOrDefault(t => t.TicketID == id);

            if (ticket == null)
            {
                return HttpNotFound();
            }

            var viewModel = new TicketViewModel
            {
                TicketID = ticket.TicketID,
                Username = ticket.User.FullName,
                MovieTitle = ticket.Showtime.Movy.Title,
                ShowDate = ticket.Showtime.ShowDate ?? DateTime.Now,
                StartTime = ticket.Showtime.StartTime ?? TimeSpan.Zero,
                Room = ticket.Showtime.Room,
                Price = ticket.Price ?? 0,
                PurchaseDate = ticket.PurchaseDate ?? DateTime.Now,
                SeatNumber = ticket.Seat.SeatNumber, // Lấy thông tin ghế
                PaymentConfirmation = (bool)ticket.PaymentConfirmation,
                PaymentMethod = ticket.PaymentMethod ?? "Chưa Thanh Toán"
            };

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult EditTicket(TicketViewModel ticketViewModel)
        {
            if (ModelState.IsValid)
            {
                var ticket = _context.Tickets.Include(t => t.Showtime).FirstOrDefault(t => t.TicketID == ticketViewModel.TicketID);
                if (ticket != null)
                {
                    ticket.Price = ticketViewModel.Price;
                    ticket.Seat.SeatNumber = ticketViewModel.SeatNumber; // Cập nhật ghế
                    ticket.Showtime.Room = ticketViewModel.Room; 
                    ticket.PaymentConfirmation = ticketViewModel.PaymentConfirmation;
                    ticket.PaymentMethod = ticketViewModel.PaymentMethod;

                    _context.SaveChanges();
                    return RedirectToAction("TicketsList");
                }
            }

            return View(ticketViewModel);
        }


        // Xóa vé
        [HttpPost]
         public ActionResult DeleteTicket(int id)
          {
                    var ticket = _context.Tickets.Find(id);
                    if (ticket != null)
                    {
                        _context.Tickets.Remove(ticket);
                        _context.SaveChanges();
                    }
                    return RedirectToAction("TicketsList");
         }


        //tạo vé 
        public ActionResult CreateTicket()
        {
            var userId = Session["UserID"];
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var model = new CreateTicketViewModel
            {
                Movies = _context.Movies
                    .Where(m => m.Showtimes.Any())  // Kiểm tra phim có suất chiếu
                    .Include(m => m.Showtimes)      // Nạp suất chiếu của phim
                    .ToList()
            };

            return View(model);
        }

        // Nếu bạn muốn lấy danh sách các suất chiếu theo từng phim khi người dùng chọn phim
        public JsonResult GetShowtimesByMovie(int movieId)
        {

            var showtimes = _context.Showtimes
                .Where(s => s.MovieID == movieId)
                .Select(s => new
                {
                    s.ShowtimeID,
                    s.StartTime,
                    s.ShowDate,
                    s.Room,
                    s.Movy.TicketPrice
                })
                .ToList();

            return Json(showtimes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSeats(int showtimeID)
        {
            // Lấy danh sách ghế cho một suất chiếu
            var seats = _context.Seats
                .Where(s => s.ShowtimeID == showtimeID)
                .Select(s => new
                {
                    s.SeatID,
                    s.SeatNumber,
                    s.IsAvailable
                })
                .ToList();

            return Json(seats, JsonRequestBehavior.AllowGet);  // Trả về danh sách ghế
        }


    }


}
