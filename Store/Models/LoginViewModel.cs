using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class LoginViewModel
    {
        [DisplayName("信箱")]
        [Required(ErrorMessage ="請輸入信箱")]
        [EmailAddress(ErrorMessage ="請輸入正確Email格式")]
        public string Email { get; set; }
        [DisplayName("密碼")]
        [Required(ErrorMessage ="請輸入密碼")]
        public string Password { get; set; }
        public string recaptchaResponse { get; set; }
    }
}