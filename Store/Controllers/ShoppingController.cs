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
        //[Authorize] ajax呼叫有適用
        [HttpPost]
        public void AddCart(string productID)//ajax 發送的資料型態為字串
        {
            //if(!User.Identity.IsAuthenticated)
            //{
            //return Json(new {});//回傳json字串，讓ajax跳轉到登入畫面
                          //}
                          //else
                          //{
            dbService dbService = new dbService();
                int loginMemberID = Convert.ToInt32(Session["memberID"]);
                dbService.AddCartItem(loginMemberID, Convert.ToInt32(productID));
                
            //}

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