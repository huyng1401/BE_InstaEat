using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountViewModels
{
    public class UpdateAccountViewModel
    {
        [Required(ErrorMessage = "Password can not be empty.")]
        public string? Password { get; set; }
    }
}
