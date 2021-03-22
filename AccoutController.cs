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
        public ActionResult Index(string searchKey, int pageNumber = 1)
        {
            List<Account> list = new List<Account>();

            if (string.IsNullOrEmpty(searchKey))
            {
                list = db.Accounts.ToList();
            }
            else
            {
                list = db.Accounts.Where(n => n.Email.Contains(searchKey)).ToList();
            }

            int pageSize = 5;

            list = list.OrderBy(n => n.Email).ToPagedList(pageNumber, pageSize).ToList();

            return View(list);
        }
        // new Accout
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
            Account acc = db.Accounts.FirstOrDefault(n => n.Email == pd.Email);
            if ( acc == null)
            {
                db.Accounts.Add(pd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                 return View();
            }
           
        }
        //edit
        [HttpGet]
        public ActionResult EditAccount(int? id = null)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Account pd = db.Accounts.SingleOrDefault(n => n.AccountID == id);
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
        // delete
        [HttpGet]
        public ActionResult DeleteAccount(int? id = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Account model = db.Accounts.SingleOrDefault(n => n.AccountID == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.Accounts.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
       
    