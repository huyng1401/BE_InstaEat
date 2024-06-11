using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.CustomerViewModels
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public string? Phone { get; set; }
    }
}
