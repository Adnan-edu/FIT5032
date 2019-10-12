using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AssignmentFIT5032.Models;
using AssignmentFIT5032.Utils;
using SendGrid;
using SendGrid.Helpers.Mail;


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
                try
                {
                    String toEmail = model.ToEmail;
                    String subject = model.Subject;
                    String contents = "No Contents Wow";
                    //String contents = model.Contents;
                    EmailSender es = new EmailSender();
                    es.Send(toEmail, subject, contents);

                    ViewBag.Result = "Email has been send.";

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
            return View();
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
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
    }
}
