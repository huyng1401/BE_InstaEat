using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.TransactionViewModels
{
    public class CreateTransactionViewModel
    {
        public int? RestaurantId { get; set; }
        public int? UserId { get; set; }
        public int? ReviewId { get; set; }
        public int? Amount { get; set; }
    }
}
