using System;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.ReviewViewModels
{
    public class CreateReviewViewModel
    {
        [Required(ErrorMessage = "CustomerId is required")]
        public int? CustomerId { get; set; }
        [Required(ErrorMessage = "RestaurantId is required")]
        public int? RestaurantId { get; set; }
        [Required(ErrorMessage = "Content is required")]
        public string? Content { get; set; }
        [Required(ErrorMessage = "Image is required")]
        public string? Image { get; set; }
    }
}
