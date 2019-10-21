using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssignmentFIT5032.Models;
using System.Text;

namespace AssignmentFIT5032.Controllers
{
    public class RoomsController : Controller
    {
        private AssignmentFit5032ModelContainer db = new AssignmentFit5032ModelContainer();

        // GET: Rooms
        public ActionResult Index()
        {
            var rooms = db.Rooms.Include(r => r.Hotel);
            return View(rooms.ToList());
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,RoomName,Description,HotelId")] Room room)
        {

            //Preventation of cross site scripting attack
            StringBuilder sbContentsRoom = new StringBuilder();
            sbContentsRoom.Append(HttpUtility.HtmlEncode(room.RoomName));
            //We are allowing bold, underline and paragraph
            sbContentsRoom.Replace("&lt;b&gt;", "<b>");
            sbContentsRoom.Replace("&lt;/b&gt;", "</b>");
            sbContentsRoom.Replace("&lt;u&gt;", "<u>");
            sbContentsRoom.Replace("&lt;/u&gt;", "</u>");
            sbContentsRoom.Replace("&lt;p&gt;", "<p>");
            sbContentsRoom.Replace("&lt;/p&gt;", "</p>");

            room.RoomName = sbContentsRoom.ToString();


            StringBuilder sbContentsDesc = new StringBuilder();
            sbContentsDesc.Append(HttpUtility.HtmlEncode(room.Description));
            //We are allowing bold, underline and paragraph
            sbContentsDesc.Replace("&lt;b&gt;", "<b>");
            sbContentsDesc.Replace("&lt;/b&gt;", "</b>");
            sbContentsDesc.Replace("&lt;u&gt;", "<u>");
            sbContentsDesc.Replace("&lt;/u&gt;", "</u>");
            sbContentsDesc.Replace("&lt;p&gt;", "<p>");
            sbContentsDesc.Replace("&lt;/p&gt;", "</p>");
            room.Description = sbContentsDesc.ToString();

            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name", room.HotelId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name", room.HotelId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoomName,Description,HotelId")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name", room.HotelId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
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
