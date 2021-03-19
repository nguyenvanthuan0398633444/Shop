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
    [Authorize(Roles = "Supplier")]
    public class SupplierController : Controller
    {
        // GET: Supplier
        ShopEntityDb db =new ShopEntityDb();
        public ActionResult Index(int? page, string seach)
        {
            List<Supplier> list;
            if (seach == null)
            {
                list = db.Suppliers.ToList();
            }
            else
            {
                list = db.Suppliers.Where(n => n.SupplierName.Contains(seach)).ToList();
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list.OrderBy(n => n.SupplierID).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult NewSupplier()
        {
            return View();
        }
        [ValidateInput(false)]// tắt kiểm tra dữ liệu đầu vào
        [HttpPost]
        public ActionResult NewSupplier(Supplier pd)
        {
            db.Suppliers.Add(pd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditSupplier(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Supplier pd = db.Suppliers.SingleOrDefault(n => n.SupplierID == id);
            if (pd == null)
            {
                return HttpNotFound();
            }

            return View(pd);
        }
        [HttpPost]
        public ActionResult EditSupplier(Supplier model)
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
        public ActionResult DeleteSupplier(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier model = db.Suppliers.SingleOrDefault(n => n.SupplierID == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.Suppliers.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}