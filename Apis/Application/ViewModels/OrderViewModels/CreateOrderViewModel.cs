using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.OrderViewModels
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "RestaurantId is required")]
        public int? RestaurantId { get; set; }
        [Required(ErrorMessage = "PackageId is required")]
        public int? PackageId { get; set; }
    }
}
