using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ShopWebApplication.Models;

namespace ShopWebApplication.Controllers
{
    public class AdminController : Controller
    {
        ShopEntityDb db = new ShopEntityDb();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            string email = f["Email"].ToString();
            string pass = f["Password"].ToString();
            // lay thoong tin dang nhap
            Account acc = db.Accounts.FirstOrDefault(n => (n.Email == email && n.Password == pass));
            if (acc != null)
            {
                // lay quyen
                var lstPower = db.Account_Power.Where(n => n.IDAccountType == acc.IDAccountType);


                string power = "";
                if (lstPower.Count() != 0)
                {
                    foreach (var item in lstPower)
                    {
                        power += item.IDPower + ",";
                    }
                }

                power = power.Substring(0, power.Length - 1);
                Powerful(acc.Email, power);
                Session["Admin"] = acc;
                return RedirectToAction("Index", "Dashboard");
                //return Content("<scrip>window.location.reload();</scrip>");
            }
            else
            {
                ViewBag.error = "Tài khoản hoặc mật khẩu sai";
                return View();
            }          
        }
        public void Powerful(string acc, string power)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                acc,
                DateTime.Now,
                DateTime.Now.AddHours(3),
                false,
                power,
                FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        [HttpGet]
        public ActionResult LogOut()
        { 

            Session["Admin"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult Erro()
        {
            return View();
        }
    }
}
