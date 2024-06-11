using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.OrderViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        public int? RestaurantId { get; set; }

        public int? PackageId { get; set; }

        public DateTime? OrderDate { get; set; }
    }
}
