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
        public ActionResult Register()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer ac)
        {
            if(db.Customers.FirstOrDefault(n => n.Email == ac.Email )== null)
            {
                db.Customers.Add(ac);
                db.SaveChanges();
                Session["taiKhoan"] = ac;
                return RedirectToAction("Index", "home");
            }
            else
            {
                ViewBag.error = "Email đã được đăng kí";
                return View();
            }
           
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
            Customer ac = db.Customers.SingleOrDefault(n => n.Email == email && n.Password == pass);
            if (ac != null)
            {
                Session["taiKhoan"] = ac;                
                return RedirectToAction("Index", "home");
            }
            ViewBag.error = "Tài khoảng hoặc mật khẩu không đúng";
            return View();
        }
        public ActionResult LogOut()
        {
            Session["taiKhoan"] = null;
            Session["Cart"] = null;
            return RedirectToAction("Index", "home");
        }
    }
}