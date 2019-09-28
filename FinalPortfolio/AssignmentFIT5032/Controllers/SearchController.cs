using AssignmentFIT5032.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace AssignmentFIT5032.Controllers
{
    public class SearchController : Controller
    {
        private AssignmentFit5032ModelContainer db = new AssignmentFit5032ModelContainer();
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchRooms()
        {
            var rooms = db.Rooms.Include(r => r.Hotel);
            return View(rooms.ToList());
        }
        public ActionResult SearchForBooking()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchForBooking([Bind(Include = "CheckInDate,CheckOutDate,IsMealRequired")] Booking booking)
        {
            //Adding booking constraint
            var already_booked = db.Bookings.Where(r => ((booking.CheckInDate >= r.CheckInDate) && (booking.CheckInDate <= r.CheckOutDate) || (booking.CheckOutDate >= r.CheckInDate) && (booking.CheckOutDate <= r.CheckOutDate))).Include(r=> r.Room).ToList<Booking>();
            var rooms = db.Rooms.SqlQuery("SELECT * FROM dbo.Rooms").ToList();
            if (rooms != null)
            {
                foreach (var bookedRoom in already_booked)
                {
                    if (rooms.Contains(bookedRoom.Room))
                    {
                        rooms.Remove(bookedRoom.Room);
                    }
                }
                var totalEmpty = rooms;
            }

            //var rooms = db.Rooms.SqlQuery("SELECT * FROM dbo.Rooms WHERE Rooms.Id NOT IN (SELECT RoomId FROM dbo.Bookings WHERE ((('" + booking.CheckInDate + "' >= Convert.ToDateTime(CheckInDate)) AND ('" + booking.CheckInDate + "' <= Convert.ToDateTime(CheckOutDate))) OR (('" + booking.CheckOutDate + "' >= Convert.ToDateTime(CheckInDate)) AND ('" + booking.CheckOutDate + "' <= Convert.ToDateTime(CheckOutDate)))))").ToList();
          
            Console.WriteLine("Already Booked: " + already_booked.Count());
            Booking booking1 = new Booking();
            var userId = User.Identity.GetUserId().ToString();
            booking1.AspNetUser = db.AspNetUsers.Single(u => u.Id.ToString() == userId);
            //DateTime dateTime1 = new DateTime(booking.CheckInDate.Year, booking.CheckInDate.Month, booking.CheckInDate.Day);
            //DateTime dateTime2 = new DateTime(booking.CheckOutDate.Year, booking.CheckOutDate.Month, booking.CheckOutDate.Day);
            booking1.CheckInDate = booking.CheckInDate;
            booking1.CheckOutDate = booking.CheckOutDate;
            booking1.IsMealRequired = true;
            booking1.RoomId = 1;
            //booking1.Id = 1;
            db.Bookings.Add(booking1);
            db.SaveChanges();
            return View();
        }

    }
}