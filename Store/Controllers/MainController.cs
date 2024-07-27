﻿using Store.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<ActionResult> Product(int pageNow = 1)
        {
                var ProductList = await dbService.GetProductList(pageNow);
                ViewBag.TotalPages = await dbService.GetProductTotalPage();
                return View(ProductList);         
        }


    }
}