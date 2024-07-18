using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class RegisterViewModel
    {
        [DisplayName("使用者名稱")]
        [Required(ErrorMessage ="請輸入使用者名稱")]
        public string UserName {  get; set; }
        [DisplayName("密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
        [DisplayName("再次確認密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "請再次輸入密碼")]
        [Compare("Password",ErrorMessage ="密碼不一致")]
        public string ComparePassword { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "請輸入Email")]
        [EmailAddress(ErrorMessage ="請輸入正確Email格式")]
        public string Email { get; set; }
        public string recaptchaResponse { get; set; }

    }
}