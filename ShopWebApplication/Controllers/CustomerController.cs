using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopWebApplication.Models;
using PagedList;

namespace ShopWebApplication.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        // GET: Customer
        ShopEntityDb db = new ShopEntityDb();
        // Index Customer
        public ActionResult Index(int? page, string seach)
        {
            List<Customer> list;
            if (seach == null)
            {
                list = db.Customers.ToList();
            }
            else
            {
                list = db.Customers.Where(n => n.CustomerName.Contains(seach)).ToList();
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list.OrderBy(n => n.CustomerID).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult NewCustomer()
        {

            return View();
        }
        [ValidateInput(false)]// tắt kiểm tra dữ liệu đầu vào
        [HttpPost]
        public ActionResult NewCustomer(Customer pd)
        {
            db.Customers.Add(pd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditCustomer(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Customer pd = db.Customers.SingleOrDefault(n => n.CustomerID == id);
            if (pd == null)
            {
                return HttpNotFound();
            }

            return View(pd);
        }
        [HttpPost]
        public ActionResult EditCustomer(Customer model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult DeleteCustomer(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer model = db.Customers.SingleOrDefault(n => n.CustomerID == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.Customers.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer ac)
        {
            NewCustomer(ac);
            Session["taiKhoan"] = ac;
            return RedirectToAction("Index", "home");
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
                //return Content("<scrip>window.location.reload();</scrip>");
                return RedirectToAction("Index", "home");
            }
            //return Content("Tài khoản hoặc mật khẩu sai");
            ViewBag.thongbao = "Tài khoảng hoặc mật khẩu không đúng";
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