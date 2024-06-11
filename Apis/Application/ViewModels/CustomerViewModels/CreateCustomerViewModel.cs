using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.CustomerViewModels
{
    public class CreateCustomerViewModel
    {
        [Required(ErrorMessage = "Customer name is required")]
        public string? CustomerName { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string? Phone { get; set; }
    }
}
