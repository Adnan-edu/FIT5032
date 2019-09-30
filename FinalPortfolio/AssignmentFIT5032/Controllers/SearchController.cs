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
        //No one can access this method make this private
        public ActionResult SearchRooms(List<Room> roomsEmpty)
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
        public ActionResult SearchForBooking(RoomBookingModel BookingModel)
        {
            Booking booking = BookingModel.ApplyBooking;
            //Adding booking constraint
            var already_booked = db.Bookings.Where(r => ((booking.CheckInDate >= r.CheckInDate) && (booking.CheckInDate <= r.CheckOutDate) || (booking.CheckOutDate >= r.CheckInDate) && (booking.CheckOutDate <= r.CheckOutDate))).Include(r=> r.Room).ToList<Booking>();
            //var rooms = db.Rooms.SqlQuery("SELECT * FROM dbo.Rooms").ToList();
            var rooms = db.Rooms.Include(r => r.Hotel).ToList();
            if (rooms != null)
            {
                foreach (var bookedRoom in already_booked)
                {
                    if (rooms.Contains(bookedRoom.Room))
                    {
                        rooms.Remove(bookedRoom.Room);
                    }
                }
                BookingModel.Rooms = rooms;
                return View(BookingModel);
            }

            //Following codes were used to test whether booking data was saved to the database or not.

            /*Console.WriteLine("Already Booked: " + already_booked.Count());
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
            db.SaveChanges();*/

            //End of testing data

            return View();
        }
        public ActionResult ConfirmBooking(int id, DateTime CheckInDate, DateTime CheckOutDate, bool IsMealRequired)
        {
            Booking booking1 = new Booking();
            var userId = User.Identity.GetUserId().ToString();
            booking1.AspNetUser = db.AspNetUsers.Single(u => u.Id.ToString() == userId);
            //DateTime dateTime1 = new DateTime(booking.CheckInDate.Year, booking.CheckInDate.Month, booking.CheckInDate.Day);
            //DateTime dateTime2 = new DateTime(booking.CheckOutDate.Year, booking.CheckOutDate.Month, booking.CheckOutDate.Day);
            booking1.CheckInDate = CheckInDate;
            booking1.CheckOutDate = CheckOutDate;
            booking1.IsMealRequired = IsMealRequired;
            booking1.RoomId = id;
            db.Bookings.Add(booking1);
            db.SaveChanges();
            return RedirectToAction("Index","Bookings");
        }

    }
}