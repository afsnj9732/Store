using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Store.Controllers
{

    public class ValidateController : Controller
    {
        dbService dbService = new dbService();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel memberInfo)
        {
            if (ModelState.IsValid) //資料驗證
            {
                var loginMemberID = dbService.GetMemberID(memberInfo);
                if (loginMemberID != null)
                {
                    Session["memberID"] = loginMemberID;
                    FormsAuthentication.RedirectFromLoginPage(memberInfo.Email, true);//授權
                    return RedirectToAction("Index", "Main");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View(memberInfo);
            }

        }


        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel memberInfo)
        {
            if (ModelState.IsValid)
            {
                dbService.CreateMember(memberInfo);
                return RedirectToAction("Index", "Main");
            }
            else
            {
                return View(memberInfo);
            }

        }
    }
}