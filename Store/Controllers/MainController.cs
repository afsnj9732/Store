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
        dbService dbService = new dbService();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product(int pageNow = 1)
        {
            var ProductList = dbService.GetProductList(pageNow);
            ViewBag.TotalPages = dbService.GetProductTotalPage();
            return View(ProductList);
        }


    }
}