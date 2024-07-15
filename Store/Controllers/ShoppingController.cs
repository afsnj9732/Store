using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Store.Controllers
{
    public class ShoppingController : Controller
    {
        dbService dbService = new dbService();

        [Authorize]
        public ActionResult CartItemList()
        {
            int loginMemberID = Convert.ToInt32(Session["memberID"]);
            return View(dbService.MemberShoppingCart(loginMemberID).ToList());
        }

        [HttpPost]
        public ActionResult GetCartItemQuantity()
        {
            if(!User.Identity.IsAuthenticated)
            {
                int ajaxStatusCode = new HttpUnauthorizedResult().StatusCode;
                return Json(new { ajaxStatus = ajaxStatusCode });//回傳401，讓ajax跳轉到登入畫面
            }
            else
            {
                int loginMemberID = Convert.ToInt32(Session["memberID"]);
                dbService.GetCartItemQuantity(loginMemberID);
                return Json(new { CartItemQuantity = dbService.GetCartItemQuantity(loginMemberID) });

            }
        }

        [HttpPost]
        public ActionResult AddCart(string productID)//ajax 發送的資料型態為字串
        {
            if (!User.Identity.IsAuthenticated)
            {
                int ajaxStatusCode = new HttpUnauthorizedResult().StatusCode;
                return Json(new { ajaxStatus=ajaxStatusCode });//無授權，回傳401
            }
            else
            {
                int loginMemberID = Convert.ToInt32(Session["memberID"]);
                dbService.AddCartItem(loginMemberID, Convert.ToInt32(productID));
                return Json(new { });
            }

        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteCart(int cartItemID) 
        {
            dbService.DeleteCartItem(cartItemID);
            return RedirectToAction("CartItemList");
        }

        [Authorize]
        public ActionResult OrderList()
        {
            int loginMemberID = Convert.ToInt32(Session["memberID"]);
            return View(dbService.GetOrderList(loginMemberID).ToList());
        }

        [Authorize]
        public ActionResult PlaceOrder(int cartID)
        {
            dbService.CreateOrder(cartID);            
            return RedirectToAction("OrderList");
        }


    }
}