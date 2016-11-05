using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Logistics.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name="Логин")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Пароль")]
        public string Password { get; set; }
    }
}