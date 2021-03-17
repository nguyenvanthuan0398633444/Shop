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
    [Authorize(Roles = "Account")]
    public class AccoutController : Controller
    {
        // GET: Accout
        ShopEntityDb db = new ShopEntityDb();
        public ActionResult Index(int? page, string seach)
        {
            List<Account> list;
            if (seach == null)
            {
                list = db.Accounts.ToList();
            }
            else
            {
                list = db.Accounts.Where(n => n.Email.Contains(seach)).ToList();
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list.OrderBy(n => n.Email).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult NewAccount()
        {
            return View();
        }
        [ValidateInput(false)]// tắt kiểm tra dữ liệu đầu vào
        [HttpPost]
        public ActionResult NewAccount(Account pd, HttpPostedFileBase PhotoPath)
        {
            if (PhotoPath.ContentLength > 0)
            {
                //lay ten anh
                var fileName = Path.GetFileName(PhotoPath.FileName);
                // lay anh chuyen vao thu muc
                var path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);               
                PhotoPath.SaveAs(path);               
                pd.PhotoPath = fileName;                
            }
            else
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Accounts.Add(pd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditAccount(string id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Account pd = db.Accounts.SingleOrDefault(n => n.Email == id);
            if (pd == null)
            {
                return HttpNotFound();
            }

            return View(pd);
        }
        [HttpPost]
        public ActionResult EditAccount(Account model, HttpPostedFileBase PhotoPath)
        {
            if (PhotoPath.ContentLength > 0)
            {
                //lay ten anh
                var fileName = Path.GetFileName(PhotoPath.FileName);
                // lay anh chuyen vao thu muc
                var path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);
                PhotoPath.SaveAs(path);
                model.PhotoPath = fileName;
            }
            else
            {
                Response.StatusCode = 404;
                return null;
            }

            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult DeleteAccount(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Account model = db.Accounts.SingleOrDefault(n => n.Email == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.Accounts.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //[HttpGet]
        //public ActionResult Login()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Login(FormCollection f)
        //{
        //    string email = f["Email"].ToString();
        //    string pass = f["Password"].ToString();
        //    Account ac = db.Accounts.SingleOrDefault(n => n.Email == email && n.Password == pass);
        //    if (ac != null)
        //    {
        //        Session["taiKhoan"] = ac;
        //        //return Content("<scrip>window.location.reload();</scrip>");
        //    }
        //    //return Content("Tài khoản hoặc mật khẩu sai");
        //    return RedirectToAction("Index", "home");
        //}
        //public ActionResult LogOut()
        //{
        //    Session["taiKhoan"] = null;
        //    return RedirectToAction("Index", "home");
        //}

    }
}
       
    