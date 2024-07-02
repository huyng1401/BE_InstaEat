using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RequestModels
{
    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\d{10}$|^\d{11}$", ErrorMessage = "Phone number must be 10 or 11 digits.")]
        [DefaultValue("0")]
        public string? Phone { get; set; } = "0";

    }
}
