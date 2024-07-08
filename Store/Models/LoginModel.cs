using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class LoginModel
    {
        [DisplayName("信箱")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DisplayName("密碼")]
        [Required]
        public string Password { get; set; }
    }
}