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
    public class TicketsController : Controller
    {
        private CGV_BinhDuong_SquareEntities db = new CGV_BinhDuong_SquareEntities();

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.SeatID = new SelectList(db.Seats, "SeatID", "Room", ticket.SeatID);
            ViewBag.ShowtimeID = new SelectList(db.Showtimes, "ShowtimeID", "Room", ticket.ShowtimeID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", ticket.UserID);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketID,UserID,ShowtimeID,SeatID,PurchaseDate,Price")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                var tickets = db.Tickets.Where(t => t.UserID == ticket.UserID).ToList();

                // Lưu giỏ vé đã cập nhật vào session
                Session["Tickets"] = tickets;

                return RedirectToAction("Details", new { id = ticket.TicketID });
            }
            ViewBag.SeatID = new SelectList(db.Seats, "SeatID", "Room", ticket.SeatID);
            ViewBag.ShowtimeID = new SelectList(db.Showtimes, "ShowtimeID", "Room", ticket.ShowtimeID);
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Username", ticket.UserID);
            return View(ticket);
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
