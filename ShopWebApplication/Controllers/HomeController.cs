using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopWebApplication.Models;
using System.Web.Security;

namespace ShopWebApplication.Controllers
{
    
    public class HomeController : Controller
    { 
       

        ShopEntityDb db = new ShopEntityDb();
        public ActionResult Index()
        {
            List<Product> listProduct = db.Products.ToList();
            return View(listProduct);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            string email = f["Email"].ToString();
            string pass = f["Password"].ToString();
            // lay thoong tin dang nhap
            Account acc = db.Accounts.SingleOrDefault(n => n.Email == email && n.Password == pass);
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
                //return Content("<scrip>window.location.reload();</scrip>");
            }
            //return Content("Tài khoản hoặc mật khẩu sai");
            return RedirectToAction("Index", "home");
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
        public ActionResult LogOut()
        {
            Session["Account"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "home");
        }
        public ActionResult Erro()
        {
            return View();
        }
    }
}