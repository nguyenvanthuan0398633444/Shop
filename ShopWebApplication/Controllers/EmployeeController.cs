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
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {

        // GET: Employee
        ShopEntityDb db =new ShopEntityDb();
        public ActionResult Index(int? page, string seach)
        {
            List<Employee> list;
            if (seach == null)
            {
                list = db.Employees.ToList();
            }
            else
            {
                list = db.Employees.Where(n => n.EmployeeName.Contains(seach)).ToList();
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(list.OrderBy(n => n.EmployeeID).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult NewEmployee()
        {

            return View();
        }
        [ValidateInput(false)]// tắt kiểm tra dữ liệu đầu vào
        [HttpPost]
        public ActionResult NewEmployee(Employee pd)
        {
            db.Employees.Add(pd);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditEmployee(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Employee pd = db.Employees.SingleOrDefault(n => n.EmployeeID == id);
            if (pd == null)
            {
                return HttpNotFound();
            }

            return View(pd);
        }
        [HttpPost]
        public ActionResult EditEmployee(Employee model)
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
        public ActionResult DeleteEmployee(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee model = db.Employees.SingleOrDefault(n => n.EmployeeID == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.Employees.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}