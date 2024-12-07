using System.Collections.Generic;
using System;
using System.Linq;
using System.Web.Mvc;
using CGVMovieWeb.Models;
using System.Data.Entity;

namespace CGVMovieWeb.Controllers
{
    public class SeatsController : Controller
    {
        private CGV_BinhDuong_SquareEntities db = new CGV_BinhDuong_SquareEntities();// Thay YourDbContext bằng tên DbContext của bạn

        // GET: Seats/Select
        public ActionResult Select(int showtimeId)
        {
            var showtime = db.Showtimes.Find(showtimeId);
            var movie = showtime.Movy; 

            var availableSeats = db.Seats
                .Where(s => s.ShowtimeID == showtimeId)
                .ToList();

            var model = new SelectSeatViewModel
            {
                ShowtimeID = showtimeId,
                MovieTitle = movie.Title,
                AvailableSeats = availableSeats,
                TicketPrice = (decimal)movie.TicketPrice,
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult DatVe(int showtimeId, decimal ticketPrice, string selectedSeats, bool PaymentConfirmation,string PaymentMethod)
        {
            if (Session["UserID"] == null)
            {
                // Xử lý khi không có UserID trong Session
                return RedirectToAction("Login", "Account");
            }

            var userId = (int)Session["UserID"];

            var seatNumbers = selectedSeats.Split(',').ToList();

            foreach (var seatNumber in seatNumbers)
            {
                var seat = db.Seats.FirstOrDefault(s => s.SeatNumber == seatNumber && s.ShowtimeID == showtimeId && s.IsAvailable == true);
                if (seat != null)
                {
                    var ticket = new Ticket
                    {
                        UserID = userId,
                        ShowtimeID = showtimeId,
                        SeatID = seat.SeatID,
                        Price = ticketPrice,
                        PurchaseDate = DateTime.Now,
                        PaymentConfirmation = PaymentConfirmation,
                        PaymentMethod = PaymentMethod
                    };
                    db.Tickets.Add(ticket);

                    // Cập nhật trạng thái ghế
                    seat.IsAvailable = false;
                    db.Entry(seat).State = EntityState.Modified;
                }
            }

            db.SaveChanges();
            // Tạo biên lai để hiển thị
            var receiptModel = new ReceiptViewModel
            {
                MovieTitle = db.Showtimes.Find(showtimeId).Movy.Title,
                TicketPrice = ticketPrice,
                SelectedSeats = seatNumbers,
                PurchaseDate = DateTime.Now,
                PaymentConfirmation = PaymentConfirmation,
                PaymentMethod = PaymentMethod
            };
            if (Session["Role"] != null && Session["Role"].ToString() == "Staff")
            {
                // Nếu là Staff, chuyển hướng đến phương thức  CreateTicket của AdminController
                return RedirectToAction("TicketsList", "Admin");
            }

            return View("Receipt", receiptModel); // Chuyển hướng đến view Receipt
        }

    }
}
