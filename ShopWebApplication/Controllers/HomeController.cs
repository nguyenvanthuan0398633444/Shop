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
    }
}