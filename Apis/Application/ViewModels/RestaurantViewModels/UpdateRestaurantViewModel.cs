using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RestaurantViewModels
{
    public class UpdateRestaurantViewModel
    {
        [Required(ErrorMessage = "Restaurant name is required")]
        public string? RestaurantName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Open time is required")]
        [DataType(DataType.Time)]
        public TimeSpan? OpenTime { get; set; }

        [Required(ErrorMessage = "Close time is required")]
        [DataType(DataType.Time)]
        public TimeSpan? CloseTime { get; set; }
    }
}
