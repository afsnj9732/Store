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
        // GET: Shopping
        dbService dbService = new dbService();
        [Authorize]
        public ActionResult CartItemList()
        {
            int memberID = Convert.ToInt32(Session["memberID"]);
            //dbService dbService = new dbService();
            return View(dbService.MemberShoppingCart(memberID));
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
                //dbService dbService = new dbService();
                int memberID = Convert.ToInt32(Session["memberID"]);
                dbService.GetCartItemQuantity(memberID);
                return Json(new { CartItemQuantity = dbService.GetCartItemQuantity(memberID) });

            }
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
                //dbService dbService = new dbService();
                int loginMemberID = Convert.ToInt32(Session["memberID"]);
                dbService.AddCartItem(loginMemberID, Convert.ToInt32(productID));
                return Json(new { });
            }

        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteCart(int cartItemID) 
        {
            //dbService dbService= new dbService();
            dbService.DeleteCartItem(cartItemID);
            return RedirectToAction("CartItemList");
        }

        [Authorize]
        public ActionResult OrderList()
        {
            int memberID = Convert.ToInt32(Session["memberID"]);
            //dbService dbService = new dbService();            
            return View(dbService.GetOrderList(memberID));
        }

        [Authorize]
        public ActionResult PlaceOrder(int cartID)
        {
            dbService.CreateOrder(cartID);            
            return RedirectToAction("OrderList");
        }


    }
}