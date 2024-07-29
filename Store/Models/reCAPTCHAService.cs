using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Store.Models
{
    public class reCAPTCHAService
    {
        public async Task<bool> VerifyRecaptcha(string recaptchaResponse)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string secretKey = ConfigurationManager.AppSettings["reCaptchaSK"]; ;
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