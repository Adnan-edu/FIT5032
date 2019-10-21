using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssignmentFIT5032.Models;

namespace AssignmentFIT5032.Controllers
{
    public class BookingsController : Controller
    {
        private AssignmentFit5032ModelContainer db = new AssignmentFit5032ModelContainer();

        // GET: Bookings
        public ActionResult Index()
        {
            var userIdentity = User.Identity.Name;
            var bookings = db.Bookings.Include(b => b.AspNetUser).Where(b=> b.AspNetUser.Email == userIdentity).Include(b => b.Room);
            return View(bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "RoomName");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CheckInDate,CheckOutDate,IsMealRequired,AspNetUserId,RoomId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", booking.AspNetUserId);
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "RoomName", booking.RoomId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", booking.AspNetUserId);
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "RoomName", booking.RoomId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CheckInDate,CheckOutDate,IsMealRequired,AspNetUserId,RoomId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", booking.AspNetUserId);
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "RoomName", booking.RoomId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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
        public ActionResult GiveRating(int? id)
        {
            Booking booking = db.Bookings.Find(id);
            var ratings = db.Bookings.Include(b => b.AspNetUser).Include(b => b.Room).Include(b => b.Room.Hotel);
            ViewBag.RATING = new SelectList(db.Ratings, "Id", "Number");
            ViewBag.AspNetUserId = new SelectList(db.AspNetUsers, "Id", "Email", booking.AspNetUserId);
            ViewBag.RoomId = new SelectList(db.Rooms, "Id", "RoomName", booking.RoomId);
            return View(booking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GiveRating([Bind(Include = "Id,CheckInDate,CheckOutDate,IsMealRequired,AspNetUserId,RoomId,RATING,Contents")]Booking booking)
        {
            var newBooking = booking;
            var idOfBooking = booking.Id;
            //booking.AspNetUserId = booking.AspNetUserId;
            var aspNetUserId = booking.AspNetUserId;
            if (ModelState.IsValid)
            {
                var updatedBooking = db.Bookings.Include(b => b.AspNetUser).Where(b => b.Id == booking.Id).Include(b => b.Room).ToList();
                newBooking = updatedBooking.FirstOrDefault();
                newBooking.RATING = booking.RATING;
                newBooking.Contents = booking.Contents;
                //Calculate rating for the hotel
                var hotel = newBooking.Room.Hotel;

                newBooking.IsRatingGiven = true;
                newBooking.IsMealRequired = booking.IsMealRequired;

                var calculatedRating = this.CalculateRatings(newBooking.RATING);
                hotel.Rating = calculatedRating;
                
                db.Entry(hotel).State = EntityState.Modified;
                db.Entry(newBooking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RATING = new SelectList(db.Ratings, "Id", "Number");
            return View(booking);
        }
        private double CalculateRatings(int? rating)
        {
            var bookingRating = db.Bookings.Where(b => b.RATING > 0).ToList();

            var countRating = bookingRating.Count() + 1;

            int? sumRating = 0;
            foreach (Booking bookRat in bookingRating)
            {
                    sumRating = sumRating + bookRat.RATING;
            }
            if (rating > 0)
            {
                sumRating = sumRating + rating;
            }
            return (double)sumRating / countRating;
        }
    }
}
