using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountViewModels
{
    public class UpdateAccountViewModel
    {
        [Required(ErrorMessage = "Name can not be empty.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Password can not be empty.")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Role can not be empty.")]
        public int? RoleId { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\d{10}$|^\d{11}$", ErrorMessage = "Phone number must be 10 or 11 digits.")]
        [DefaultValue("0")]
        public string? Phone { get; set; } = "0";
    }
}
