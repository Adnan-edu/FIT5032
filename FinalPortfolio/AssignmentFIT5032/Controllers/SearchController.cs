using AssignmentFIT5032.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

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

    }
}