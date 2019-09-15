using FIT5032AssignmentPortfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FIT5032AssignmentPortfo.Controllers
{
    public class HomeController : Controller
    {
        private FIT5032PortfolioModelContainer db = new FIT5032PortfolioModelContainer();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserAccounts userAccount)
        {
            if (ModelState.IsValid)
            {
                userAccount.UserRole = db.UserRoles.Single(u => u.RoleName == "CUSTOMER" && u.Id==8); //By default the person who would register will be treated as a customer
                db.UserAccounts.Add(userAccount);
                db.SaveChanges();
                ModelState.Clear();
                ViewBag.Message = userAccount.Email + " successfully registered.";
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
        public ActionResult Login(UserAccounts userAccount)
        {
            var usr = db.UserAccounts.Single(u => u.Email == userAccount.Email && u.Password == userAccount.Password);
            if (usr != null)
            {
                Session["RoleName"] = usr.UserRole.RoleName.ToString(); 
                Session["Email"] = usr.Email.ToString();
                //return RedirectToAction("Dashboard");
                return RedirectToAction("Dashboard", "Dashboard", new { area = "" });
            }
            else
            {
                ModelState.AddModelError("", "Email or Password is wrong.");
                return View(userAccount);
            }

        }

        [AllowAnonymous]
        public ActionResult Dashboard()
        {
            if (Session["Email"] != null)
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session["RoleName"] = null;
            Session["UserId"] = null;
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult CreateManager()
        {
            return View();
        }
    }
}