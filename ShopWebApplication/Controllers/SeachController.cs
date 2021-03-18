using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopWebApplication.Models;

namespace ShopWebApplication.Controllers
{
    public class SeachController : Controller
    {
        // GET: Seach
        ShopEntityDb db =new ShopEntityDb();
        public ActionResult Seach(string keySeach)
        {
            var lstSeach = db.Products.Where(n => n.ProductName.Contains(keySeach));
            return View(lstSeach);
        }
    }
}