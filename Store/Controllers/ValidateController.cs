using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Store.Controllers
{

    public class ValidateController : Controller
    {
        dbService dbService = new dbService();
        reCAPTCHAService reCAPTCHAService = new reCAPTCHAService();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel memberInfo)
        {
            if (ModelState.IsValid) //資料驗證
            {
                var getVerifyRecaptcha = reCAPTCHAService.VerifyRecaptcha(memberInfo.recaptchaResponse);
                var getMemberID =  dbService.GetMemberID(memberInfo.Email, memberInfo.Password);

                bool ifCaptcha = await getVerifyRecaptcha;

                if (ifCaptcha == false)
                {
                    ViewBag.LoginError = "你有點像機器人，請重新嘗試";
                    return View(memberInfo);
                }

                int? loginMemberID = await getMemberID;

                if (loginMemberID != null)
                {
                    ViewBag.LoginError = "";
                    Session["memberID"] = loginMemberID;
                    FormsAuthentication.SetAuthCookie(memberInfo.Email, false);//授權
                    return RedirectToAction("Index", "Main");
                }
                else
                {
                    ViewBag.LoginError = "帳號或密碼輸入錯誤";
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
            Session.Abandon();
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel memberInfo)
        {
            if (ModelState.IsValid)
            {
                var getVerifyRecaptcha = reCAPTCHAService.VerifyRecaptcha(memberInfo.recaptchaResponse);
                var checkMemberExist =  dbService.CheckMemberExist(memberInfo.Email);

                bool ifCaptcha = await getVerifyRecaptcha;

                if (ifCaptcha == false)
                {
                    ViewBag.RegisterError = "你有點像機器人，請重新嘗試";
                    return View(memberInfo);
                }

                bool memberExist = await checkMemberExist;

                if (memberExist)
                {
                    ViewBag.RegisterError = "Email已註冊";
                    return View(memberInfo);
                }
                else
                {                   
                    dbService.CreateMember(memberInfo);
                    int? loginMemberID = await dbService.GetMemberID(memberInfo.Email, memberInfo.Password);
                    Session["memberID"] = loginMemberID;
                    FormsAuthentication.SetAuthCookie(memberInfo.Email, true);//授權                    
                    TempData["Register"] = "註冊成功";
                    return RedirectToAction("Index", "Main");
                }

            }
            else
            {
                return View(memberInfo);
            }

        }

        

    }
}

