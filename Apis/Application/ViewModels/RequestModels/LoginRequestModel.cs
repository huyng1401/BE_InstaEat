using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RequestModels
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Email can not be empty.")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Password can not be empty.")]
        public string? Password { get; set; }
    }
}
