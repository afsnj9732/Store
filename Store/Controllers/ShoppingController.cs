﻿using Store.Models;
using Stripe;
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
        public ActionResult CartItemList()
        {
            ViewBag.StripePublicKey = "pk_test_51Pesm02La0H5PIYutJuIiEkUXFXagRryVad9x9fhP4WDFqjjzPhO0shoZqRQMhdXxvtEfGxmb7gruzpjkVCKDXA00012AZZjHg";
            int loginMemberID = Convert.ToInt32(Session["memberID"]);
            var cartItemList = dbService.MemberShoppingCart(loginMemberID).ToList();
            return View(cartItemList);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(string stripeToken,int cartID)
        {
            int newOrderID = dbService.CreateOrder(cartID);  
            int orderPrice = dbService.GetOrderPrice(newOrderID);
            StripeConfiguration.ApiKey = "sk_test_51Pesm02La0H5PIYu13IyQZhzBEoV4xiuU2DY225yu7pZOezKhR1NU1bULY5KLm2CN6b1JzfmZK4SuFfDhIyQ8yHx00oqPP1pYF";
            var options = new ChargeCreateOptions
            {
                Amount = orderPrice*100, 
                Currency = "twd",
                Description = "付款",
                Source = stripeToken,
            };
            var service = new ChargeService();
            var charge = service.Create(options);
            return RedirectToAction("OrderList");
        }


    }
}