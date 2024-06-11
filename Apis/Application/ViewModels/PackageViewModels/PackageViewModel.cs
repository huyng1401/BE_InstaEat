using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.PackageViewModels
{
    public class PackageViewModel
    {
        public int PackageId { get; set; }
        public string? PackageName { get; set; }
        public decimal Price { get; set; }
        public int Point { get; set; }
    }
}
