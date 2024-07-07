using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product()
        {
            var ProductList = new dbStoreEntities().tProducts.ToList();
            return View(ProductList);
        }

 

        public ActionResult Cart()
        {
            var CartList = new dbStoreEntities().tCart.ToList();
            return View(CartList);
        }
    }
}