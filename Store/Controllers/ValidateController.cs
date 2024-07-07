using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Login(tMembers memberInfo)
        {
            if (ModelState.IsValid)
            {
                dbService dbService = new dbService();
                if (dbService.CheckMember(memberInfo))
                {
                    //Session["Customer"]= memberInfo.UserName;
                    return RedirectToAction("Index", "Main");
                }
                else
                {
                    ViewBag.MemberInfo = "不存在的帳號密碼";
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