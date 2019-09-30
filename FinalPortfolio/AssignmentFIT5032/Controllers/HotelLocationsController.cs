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
    public class HotelLocationsController : Controller
    {
        private AssignmentFit5032ModelContainer db = new AssignmentFit5032ModelContainer();

        // GET: HotelLocations
        public ActionResult Index()
        {
            var hotelLocations = db.HotelLocations.Include(h => h.Hotel);
            return View(hotelLocations.ToList());
        }

        // GET: HotelLocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HotelLocation hotelLocation = db.HotelLocations.Find(id);
            if (hotelLocation == null)
            {
                return HttpNotFound();
            }
            return View(hotelLocation);
        }

        // GET: HotelLocations/Create
        public ActionResult Create()
        {
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name");
            return View();
        }

        // POST: HotelLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,HotelId,Latitude,Longitude")] HotelLocation hotelLocation)
        {
            if (ModelState.IsValid)
            {
                db.HotelLocations.Add(hotelLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name", hotelLocation.HotelId);
            return View(hotelLocation);
        }

        // GET: HotelLocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HotelLocation hotelLocation = db.HotelLocations.Find(id);
            if (hotelLocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name", hotelLocation.HotelId);
            return View(hotelLocation);
        }

        // POST: HotelLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,HotelId,Latitude,Longitude")] HotelLocation hotelLocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotelLocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HotelId = new SelectList(db.Hotels, "Id", "Name", hotelLocation.HotelId);
            return View(hotelLocation);
        }

        // GET: HotelLocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HotelLocation hotelLocation = db.HotelLocations.Find(id);
            if (hotelLocation == null)
            {
                return HttpNotFound();
            }
            return View(hotelLocation);
        }

        // POST: HotelLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HotelLocation hotelLocation = db.HotelLocations.Find(id);
            db.HotelLocations.Remove(hotelLocation);
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
