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
            bool ifCaptcha = await VerifyRecaptcha(memberInfo.recaptchaResponse);
            if (ModelState.IsValid && ifCaptcha) //資料驗證
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
        public async Task<ActionResult> Register(RegisterViewModel memberInfo)
        {
            bool ifCaptcha = await VerifyRecaptcha(memberInfo.recaptchaResponse);
            if (ModelState.IsValid && ifCaptcha)
            {
                var memberExist = dbService.CheckMemberExist(memberInfo.Email);
                if (memberExist)
                {
                    ViewBag.Exist = "Email已註冊";
                    return View(memberInfo);
                }
                else
                {
                    dbService.CreateMember(memberInfo);
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
                return false;
            }
        }

    }
}

