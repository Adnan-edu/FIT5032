using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AssignmentFIT5032.Models;
using AssignmentFIT5032.Utils;
using SendGrid;
using SendGrid.Helpers.Mail;
using Newtonsoft.Json;

namespace AssignmentFIT5032.Controllers
{
    public class HotelsController : Controller
    {
        private AssignmentFit5032ModelContainer db = new AssignmentFit5032ModelContainer();

        // GET: Hotels
        public ActionResult Index()
        {
            return View(db.Hotels.ToList());
        }

        public ActionResult Send_BulkEmail()
        {
            return View(new BulkEmailViewModel());
        }

        [HttpPost]
        public ActionResult Send_BulkEmail(BulkEmailViewModel model)
        {
            if (model.ToEmail == null || model.Subject == null || model.Contents == null)
            {
                ViewBag.Result = "Please provide the required field.";
                return View();
            }
            try
            {
                String toEmail = model.ToEmail;
                String subject = model.Subject;
                String contents = model.Contents;
                //String contents = model.Contents;

                string path = Server.MapPath("~/App_Data/uploads");
                string fileName = Path.GetFileName(model.AttachedFile.FileName);
                string fullPath = Path.Combine(path, fileName);
                model.AttachedFile.SaveAs(fullPath);

                BulkEmailSender bulkEmailSender = new BulkEmailSender();
                bulkEmailSender.Send(toEmail, subject, contents, model, fullPath);

                ViewBag.Result = "Email has been send.";

                ModelState.Clear();

                return View(new BulkEmailViewModel());
            }
            catch (Exception e)
            {
                return View();
            }


            return View();
        }

        public ActionResult Send_Email()
        {
            return View(new SendEmailViewModel());
        }

        [HttpPost]
        public ActionResult Send_Email(SendEmailViewModel model)
        {
            if (model.ToEmail == null || model.Subject == null || model.Contents == null)
            {
                @ViewBag.Message = "Please privide the required fields.";
                return View();
            }
                try
                {
                    String toEmail = model.ToEmail;
                    String subject = model.Subject;
                    String contents = model.Contents;
                    EmailSender es = new EmailSender();
                    es.Send(toEmail, subject, contents);

                    ViewBag.Message = "Email has been sent.";

                    ModelState.Clear();

                    return View(new SendEmailViewModel());
                }
                catch(Exception e)
                {
                    return View();
                }
            

            return View();
        }

        // GET: Hotels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // GET: Hotels/Create
        public ActionResult Create()
        {
            var jsonData = this.ChartVizualization();
            return View();
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address")] Hotel hotel)
        {
            if (hotel.Name == null || hotel.Address == null)
            {
                ViewBag.Message = "Hotel name or address can't be empty";
                return View();
            }
            if (ModelState.IsValid)
            {
                //During hotel creation hotel rating will be 0.0
                hotel.Rating = 0.0;
                db.Hotels.Add(hotel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hotel hotel = db.Hotels.Find(id);
            db.Hotels.Remove(hotel);
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

        public List<CustomerBookingChart> GetResult()
        {
           

            //Getting customer user role
            var userRole = db.AspNetRoles.Single(u => u.Name == "Customer");
            var UserList = db.AspNetUsers.Include(b=> b.AspNetRoles).ToList();
            var RoleList = UserList;
            var userList = db.AspNetUsers.Include(b => b.AspNetRoles).Include(b=> b.Bookings).ToList();
            List<CustomerBookingChart> BookingList = new List<CustomerBookingChart>();
            foreach(var customerBooking in userList)
            {
              //  if (customerBooking.AspNetRoles.Contains(userRole))
               // {
                    var bookingsPerCustomer = db.Bookings.Include(b => b.AspNetUser).Where(b => b.AspNetUser.Email == customerBooking.Email).Include(b => b.Room).ToList().Count();
                    //var BookingCount = db.Bookings.Where(b => b.AspNetUser.Id == customerBooking.Id).GroupBy(b=> b.AspNetUser.Id).Count();
                    CustomerBookingChart BookingChart  = new CustomerBookingChart();
                    BookingChart.CustomerEmail = customerBooking.Email;
                    BookingChart.NoOfBooking = bookingsPerCustomer;
                    BookingList.Add(BookingChart);
               // }  

            }
            return BookingList;
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ChartVizualization()
        {
            List<CustomerBookingChart> bookingList = GetResult();
            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (var bookingcount in bookingList)
            {
                dataPoints.Add(new DataPoint(bookingcount.CustomerEmail, bookingcount.NoOfBooking));
            }
            /*dataPoints.Add(new DataPoint("Customer 6", 8));
            dataPoints.Add(new DataPoint("Customer 7", 9));
            dataPoints.Add(new DataPoint("Customer 8", 9));
            dataPoints.Add(new DataPoint("Customer 11", 9));
            dataPoints.Add(new DataPoint("Customer 20", 9));
            dataPoints.Add(new DataPoint("Customer 71", 9));*/
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }
    }
}
