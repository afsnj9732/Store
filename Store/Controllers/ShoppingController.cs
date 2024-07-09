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
        
        [HttpPost]
        public ActionResult AddCart(string productID)//ajax 發送的資料型態為字串
        {
            if (!User.Identity.IsAuthenticated)
            {
                int ajaxStatusCode = new HttpUnauthorizedResult().StatusCode;
                return Json(new { ajaxStatus=ajaxStatusCode });//回傳401，讓ajax跳轉到登入畫面
            }
            else
            {
                dbService dbService = new dbService();
                int loginMemberID = Convert.ToInt32(Session["memberID"]);
                dbService.AddCartItem(loginMemberID, Convert.ToInt32(productID));
                return null;
            }

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