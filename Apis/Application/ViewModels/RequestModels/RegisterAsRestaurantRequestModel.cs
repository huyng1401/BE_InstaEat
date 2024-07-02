using Application.ViewModels.RestaurantViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RequestModels
{
    public class RegisterAsRestaurantRequestModel
    {
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Restaurant is required")]
        public CreateRestaurantViewModel Restaurant { get; set; }
    }
}
