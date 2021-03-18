using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShopWebApplication.Models;

namespace ShopWebApplication.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int count { get; set; }
        public Nullable<decimal> cost { get; set; }
        public Nullable<decimal> intoMoney { get; set; }
        public string photo { get; set; }
        
        public Cart(int ids, int counts)
        {
            using (ShopEntityDb db = new ShopEntityDb())
            {
                this.Id = ids;
                Product sp = db.Products.Single(n => n.ProductID == ids);
                this.name = sp.ProductName;
                this.cost = sp.Price.Value;
                this.photo = sp.PhotoPath;
                this.count = counts;
                this.intoMoney = cost * count;
            }
                
        }
        public Cart(int Ids)
        {
            using (ShopEntityDb db = new ShopEntityDb())
            {
                this.Id = Ids;
                Product sp = db.Products.Single(n => n.ProductID == Ids);
                this.name = sp.ProductName;
                this.cost = sp.Price.Value;
                this.photo = sp.PhotoPath;
                this.count = 1;
                this.intoMoney = cost * count;
            }

        }
        public Cart(){        }
    }
}