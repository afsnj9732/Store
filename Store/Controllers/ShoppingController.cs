using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Controllers
{
    public class ShoppingController : Controller
    {
        // GET: Shopping

        public ActionResult CartList()
        {
            return View();
        }

        public ActionResult AddCart()
        {
            return View();
        }

        public ActionResult DeleteCart() 
        {
            return View();
        }

        public ActionResult OrderList()
        {
            return View();
        }




    }
}