using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Assessment2.Models;

namespace Assessment2.Controllers
{
    [Authorize]
    public class UserAccountsController : Controller
    {
        private LoginAndRegistrationContainer db = new LoginAndRegistrationContainer();

        // GET: UserAccounts
        /* public ActionResult Index()
         {
             return View(db.UserAccounts.ToList());
         }*/

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                var usr = db.UserAccounts.Where(u => u.Email == userAccount.Email).FirstOrDefault();
                if (usr != null)
                {
                    ModelState.AddModelError("", "This email was used.Please try another email.");
                }
                else
                {
                    db.UserAccounts.Add(userAccount);
                    db.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = userAccount.FirstName + " " + userAccount.LastName + " successfully registered.";
                }
            }
                return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "UserId,FirstName,LastName,Email,Password")] UserAccount userAccount)
        {
            /*foreach (var modelStateValue in ViewData.ModelState.Values)
            {
                foreach (var error in modelStateValue.Errors)
                {
                    // Do something useful with these properties
                    var errorMessage = error.ErrorMessage;
                    var exception = error.Exception;
                    Console.WriteLine(errorMessage);
                    Console.WriteLine(exception);
                }
            }*/
            /*if (!ModelState.IsValid)
            {
                return View(userAccount);
            }*/
            var usr = db.UserAccounts.Where(u => u.Email == userAccount.Email && u.Password == userAccount.Password).FirstOrDefault();
            if (usr != null)
            {
                Session["UserId"] = usr.UserId.ToString();
                Session["Email"] = usr.Email.ToString();
                FormsAuthentication.SetAuthCookie(usr.UserId.ToString(), true);
                return RedirectToAction("LoggedIn");
            }
            else
            {
                ModelState.AddModelError("", "Email or Password is wrong.");
                return View(userAccount);
            }

        }
        [AllowAnonymous]
        public ActionResult LoggedIn()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            return RedirectToAction("Login");
        }


        // GET: UserAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount userAccount = db.UserAccounts.Find(id);
            if (userAccount == null)
            {
                return HttpNotFound();
            }
            return View(userAccount);
        }

        // GET: UserAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,FirstName,LastName,Email,UserName,Password,ConfirmPassword")] UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                db.UserAccounts.Add(userAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userAccount);
        }

        // GET: UserAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount userAccount = db.UserAccounts.Find(id);
            if (userAccount == null)
            {
                return HttpNotFound();
            }
            return View(userAccount);
        }

        // POST: UserAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,FirstName,LastName,Email,UserName,Password,ConfirmPassword")] UserAccount userAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userAccount);
        }

        // GET: UserAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAccount userAccount = db.UserAccounts.Find(id);
            if (userAccount == null)
            {
                return HttpNotFound();
            }
            return View(userAccount);
        }

        // POST: UserAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserAccount userAccount = db.UserAccounts.Find(id);
            db.UserAccounts.Remove(userAccount);
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
       
        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session["Email"] = null;
            Session["UserId"] = null;
            return RedirectToAction("Login");
        }
    }
}
