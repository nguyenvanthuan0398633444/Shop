using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopWebApplication.Models;

namespace ShopWebApplication.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        ShopEntityDb db = new ShopEntityDb();
       
        public List<Cart> getCart()
        {
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if(lstCart == null)
            {
                lstCart = new List<Cart>();
                Session["Cart"] = lstCart;
                return lstCart;
            }
            return lstCart;
        }
        public ActionResult AddCart( int ids, string url)
        {
            Product sp = db.Products.SingleOrDefault(n => n.ProductID == ids);
            if(sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //lay gio hang
            List<Cart> lsCart = getCart();
            //san pham ton tai trong gio hang
            if (lsCart != null)
            {
                Cart spChesk = lsCart.SingleOrDefault(n => n.Id == ids);
                if (spChesk != null)
                {
                    spChesk.count++;
                    spChesk.intoMoney = spChesk.count * spChesk.cost;
                    return Redirect(url);
                }
            }

            Cart itemCart = new Cart(ids);
            lsCart.Add(itemCart);
            return Redirect(url);            
        }
        public double sumProduct()
        {
            List<Cart> lsCart = Session["Cart"] as List<Cart>;
            if(lsCart == null)
            {
                return 0;
            }
            return lsCart.Sum( n => n.count) ;

        }
        public Nullable<decimal> sumMoney()
        {
            List<Cart> lsCart = Session["Cart"] as List<Cart>;
            if (lsCart == null)
            {
                return 0;
            }
            return lsCart.Sum(n => n.intoMoney);

        }
        public ActionResult CartPatial()
        {
            if(sumProduct() == 0)
            {
                ViewBag.sumProduct = 0;
                return PartialView();
            }
            ViewBag.sumProduct = sumProduct();
            return PartialView();
        }
        public ActionResult Index()
        {
            List<Cart> lstCart = getCart();
            ViewBag.sumCout = sumProduct();
            ViewBag.sumMoney = sumMoney();
            return View(lstCart);
        }
        public ActionResult EditCart(int ids)
        {
           // kiem tra gio ton tai chua
           if(Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
           // kiem tra san pham ton tai ko
            Product prd = db.Products.SingleOrDefault(n => n.ProductID == ids);
            if(prd == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // kiem tra san pham co ton tai trong gio ko
            List<Cart> lstCart = getCart();
            Cart productCart = lstCart.SingleOrDefault(n => n.Id == ids);
            if ( productCart== null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(productCart);
        }
        [HttpGet]
        public ActionResult DeleteCart(int ids)
        {
            // kiem tra gio ton tai chua
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // kiem tra san pham ton tai ko
            Product prd = db.Products.SingleOrDefault(n => n.ProductID == ids);
            if (prd == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // kiem tra san pham co ton tai trong gio ko
            List<Cart> lstCart = getCart();
            Cart productCart = lstCart.SingleOrDefault(n => n.Id == ids);
            if (productCart == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            lstCart.Remove(productCart);
            return RedirectToAction("Index", "Cart");
        }

    }
}