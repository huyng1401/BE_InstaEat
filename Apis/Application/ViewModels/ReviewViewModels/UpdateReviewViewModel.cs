using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ReviewViewModels
{
    public class UpdateReviewViewModel
    {
        [Required(ErrorMessage = "CustomerId is required")]
        public int? CustomerId { get; set; }
        [Required(ErrorMessage = "RestaurantId is required")]
        public int? RestaurantId { get; set; }
        [Required(ErrorMessage = "Content is required")]
        public string? Content { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public int? Status { get; set; }

    }
}
