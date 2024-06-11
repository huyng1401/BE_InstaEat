using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.PackageViewModels
{
    public class UpdatePackageViewModel
    {
        [Required(ErrorMessage = "Package name is required")]
        public string? PackageName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative number")]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Point must be a non-negative number")]
        public int Point { get; set; } = 0;
    }
}
