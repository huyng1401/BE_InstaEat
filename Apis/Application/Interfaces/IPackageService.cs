using Application.Commons;
using Application.ViewModels.AccountViewModels;
using Application.ViewModels.PackageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPackageService
    {
        Task<Pagination<PackageViewModel>> GetPackagesAsync(int pageIndex = 0, int pageSize = 10);
        Task<PackageViewModel?> GetPackageByIdAsync(int packageId);
        Task<PackageViewModel?> CreatePackageAsync(CreatePackageViewModel package);
        Task<bool> UpdatePackageAsync(int packageId, UpdatePackageViewModel package);
        Task<bool> DeletePackageAsync(int packageId);
    }
}
