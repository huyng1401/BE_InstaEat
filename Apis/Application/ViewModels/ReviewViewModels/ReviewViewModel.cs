using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ReviewViewModels
{
    public class ReviewViewModel
    {
        public int ReviewId { get; set; }

        public int? CustomerId { get; set; }

        public int? RestaurantId { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        public int? Status { get; set; }

        public DateTime? Created { get; set; }
    }
}
