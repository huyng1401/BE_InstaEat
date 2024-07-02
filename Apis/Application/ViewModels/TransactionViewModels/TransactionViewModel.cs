using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.TransactionViewModels
{
    public class TransactionViewModel
    {
        public int TransactionId { get; set; }
        public int? RestaurantId { get; set; }
        public int? UserId { get; set; }
        public int? ReviewId { get; set; }
        public int? Amount { get; set; }
        public DateTime? Created { get; set; }
    }
}
