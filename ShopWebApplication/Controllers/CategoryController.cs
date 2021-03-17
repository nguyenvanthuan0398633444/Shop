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
    [Authorize(Roles = "Category")]
    public class CategoryController : Controller
    {
        // GET: Category
        ShopEntityDb db = new ShopEntityDb();
        public ActionResult Index(int? page, string seach)
        {
            //var listsp = from sp in db.Categorys select sp;
            List<Category> list;
            if (seach == null)
            {
                list = db.Categories.ToList();
            }
            else
            {
                list = db.Categories.Where(n => n.CategoryName.Contains(seach)).ToList();
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list.OrderBy(n => n.CategoryID).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult NewCategory()
        {
        
            return View();
        }
        [ValidateInput(false)]// tắt kiểm tra dữ liệu đầu vào
        [HttpPost]
        public ActionResult NewCategory(Category pd)
        {
            db.Categories.Add(pd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Category pd = db.Categories.SingleOrDefault(n => n.CategoryID == id);
            if (pd == null)
            {
                return HttpNotFound();
            }
 
            return View(pd);
        }
        [HttpPost]
        public ActionResult EditCategory(Category model)
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
        public ActionResult DeleteCategory(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category model = db.Categories.SingleOrDefault(n => n.CategoryID == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.Categories.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}