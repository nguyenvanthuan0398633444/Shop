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
    [Authorize(Roles = "Product")]
    public class ProductController : Controller
    {
        // GET: Product
       ShopEntityDb db = new ShopEntityDb();
        public ActionResult Index1()
        {
            //var listsp = from sp in db.Products select sp;
            List<Product> l = db.Products.ToList();
            return View(l);
        }
        public ActionResult Index(int? page, string seach)
        {
            //var listsp = from sp in db.Products select sp;
            List<Product> list;
            if (seach == null) {
                list = db.Products.ToList();
            }
            else
            {
                list = db.Products.Where(n => n.ProductName.Contains(seach)).ToList();
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list.OrderBy(n => n.ProductID).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult NewProduct()
        {
            ViewBag.SupplierID = new SelectList(db.Suppliers.OrderBy(n => n.SupplierName), "SupplierID", "SupplierName");
            ViewBag.CategoryID = new SelectList(db.Categories.OrderBy(n => n.CategoryName), "CategoryID", "CategoryName");
            return View();
        }
        [ValidateInput(false)]// tắt kiểm tra dữ liệu đầu vào
        [HttpPost]
        public ActionResult NewProduct(Product pd, HttpPostedFileBase PhotoPath)
        {
            ViewBag.SupplierID = new SelectList(db.Suppliers.OrderBy(n => n.SupplierName), "SupplierID", "SupplierName");
            ViewBag.CategoryID = new SelectList(db.Categories.OrderBy(n => n.CategoryName), "CategoryID", "CategoryName");
            if(PhotoPath.ContentLength > 0)
            {
                //lay ten anh
                var fileName = Path.GetFileName(PhotoPath.FileName);
                // lay anh chuyen vao thu muc
                var path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);
                // neu anh co roi thi ra thong bao loi
                //if (System.IO.File.Exists(path))
                //{
                //    ViewBag.upload = "hinh da ton tai";
                //    return View();
                //}
                //else
                //{
                    //lay anh dua vao thu muc hinh anh
                    PhotoPath.SaveAs(path);
                    //Session["tenHinh"] = PhotoPath.FileName;
                    //ViewBag.tenHinh = "";
                    pd.PhotoPath = fileName;
                //}
                
            }
            else
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Products.Add(pd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Product pd = db.Products.SingleOrDefault(n =>n.ProductID == id);
            if(pd == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers.OrderBy(n => n.SupplierName), "SupplierID", "SupplierName");
            ViewBag.CategoryID = new SelectList(db.Categories.OrderBy(n => n.CategoryName), "CategoryID", "CategoryName");
            return View(pd);
        }
        [HttpPost]
        public ActionResult EditProduct(Product model, HttpPostedFileBase PhotoPath)
        {
            ViewBag.SupplierID = new SelectList(db.Suppliers.OrderBy(n => n.SupplierName), "SupplierID", "SupplierName");
            ViewBag.CategoryID = new SelectList(db.Categories.OrderBy(n => n.CategoryName), "CategoryID", "CategoryName");
            //lay ten anh
            var fileName = Path.GetFileName(PhotoPath.FileName);
            // lay anh chuyen vao thu muc
            var path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);
            //lay anh dua vao thu muc hinh anh
            PhotoPath.SaveAs(path);
            //Session["tenHinh"] = PhotoPath.FileName;
            //ViewBag.tenHinh = "";
            model.PhotoPath = fileName;

            //if (PhotoPath.ContentLength > 0)
            //{
            //    //lay ten anh
            //    var fileName = Path.GetFileName(PhotoPath.FileName);
            //    // lay anh chuyen vao thu muc
            //    var path = Path.Combine(Server.MapPath("~/Content/Image"), fileName);
            //    // neu anh co roi thi ra thong bao loi
            //    if (System.IO.File.Exists(path))
            //    {
            //        ViewBag.upload = "hinh da ton tai";
            //        return View();
            //    }
            //    else
            //    {
            //        //lay anh dua vao thu muc hinh anh
            //        PhotoPath.SaveAs(path);
            //        //Session["tenHinh"] = PhotoPath.FileName;
            //        //ViewBag.tenHinh = "";
            //        model.PhotoPath = fileName;
            //    }

            //}
            //else
            //{
            //    Response.StatusCode = 404;
            //    return null;
            //}
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //public ActionResult DeleteProduct(int? id)
        //{
        //    if (id == null)
        //    {
        //        Response.StatusCode = 404;
        //        return null;
        //    }
        //    Product pd = db.Products.SingleOrDefault(n => n.ProductID == id);
        //    if (pd == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.SupplierID = new SelectList(db.Suppliers.OrderBy(n => n.SupplierName), "SupplierID", "SupplierName");
        //    ViewBag.CategoryID = new SelectList(db.Categories.OrderBy(n => n.CategoryName), "CategoryID", "CategoryName");
        //    return View(pd);
        //}
        //[HttpPost]
        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product model = db.Products.SingleOrDefault(n => n.ProductID == id);
            if(model == null)
            {
                return HttpNotFound();
            }
            db.Products.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}