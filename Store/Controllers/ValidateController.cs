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
                bool ifCaptcha = await VerifyRecaptcha(memberInfo.recaptchaResponse);
                if (ifCaptcha == false)
                {
                    ViewBag.LoginError = "你有點像機器人，請重新嘗試";
                    return View(memberInfo);
                }
                var loginMemberID = dbService.GetMemberID(memberInfo.Email,memberInfo.Password);
                if (loginMemberID != null)
                {
                    ViewBag.LoginError = "";
                    Session["memberID"] = loginMemberID;
                    FormsAuthentication.SetAuthCookie(memberInfo.Email, true);//授權
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
                bool ifCaptcha = await VerifyRecaptcha(memberInfo.recaptchaResponse);
                if(ifCaptcha == false)
                {
                    ViewBag.RegisterError = "你有點像機器人，請重新嘗試";
                    return View(memberInfo);
                }
                var memberExist = dbService.CheckMemberExist(memberInfo.Email);
                if (memberExist)
                {
                    ViewBag.RegisterError = "Email已註冊";
                    return View(memberInfo);
                }
                else
                {
                    
                    dbService.CreateMember(memberInfo);
                    var loginMemberID = dbService.GetMemberID(memberInfo.Email,memberInfo.Password);
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

        private async Task<bool> VerifyRecaptcha(string recaptchaResponse)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string secretKey = "6LdoNBIqAAAAAFxAgPvrGLf-Li4IYWfkRl9mki3P";
                    var url = "https://www.google.com/recaptcha/api/siteverify" +
                        "?secret=" + secretKey + "&response=" + recaptchaResponse;
                    var response = await client.PostAsync(url, null);

                    string jsonString = await response.Content.ReadAsStringAsync();
                    var jsonData = JObject.Parse(jsonString);

                    return (bool)jsonData["success"];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}

