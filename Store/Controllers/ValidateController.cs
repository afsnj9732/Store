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
        // GET: Validate
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel memberInfo)
        {
            if (ModelState.IsValid) //資料驗證
            {
                dbService dbService = new dbService();
                if (dbService.CheckMember(memberInfo))
                {
                    Session["memberID"] = dbService.GetMemberID(memberInfo);
                    FormsAuthentication.RedirectFromLoginPage(memberInfo.Email, true);//授權
                    return RedirectToAction("Index", "Main");
                }
                else
                {
                    //ViewBag.MemberInfo = "不存在的帳號密碼";
                    return View();
                }
            }
            else
            {
                return View(memberInfo);
            }

        }

        public ActionResult Register()
        {
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult Register(tMembers memberInfo)
        {
            if (ModelState.IsValid)
            {
                dbService dbService = new dbService();
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