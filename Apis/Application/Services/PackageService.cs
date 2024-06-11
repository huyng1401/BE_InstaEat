using Application.Interfaces;
using Application.ViewModels.PackageViewModels;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PackageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<PackageViewModel>> GetPackagesAsync()
        {
            var packages = await _unitOfWork.PackageRepository.GetAllNotDeletedAsync();
            var result = _mapper.Map<List<PackageViewModel>>(packages);
            return result;
        }

        public async Task<PackageViewModel?> GetPackageByIdAsync(int packageId)
        {
            var package = await _unitOfWork.PackageRepository.GetByIdAsync(packageId);
            if (package != null && package.IsDeleted == false)
            {
                return _mapper.Map<PackageViewModel>(package);
            }
            return null;
        }

        public async Task<PackageViewModel?> CreatePackageAsync(CreatePackageViewModel package)
        {
            var packageObj = _mapper.Map<Package>(package);
            await _unitOfWork.PackageRepository.AddAsync(packageObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<PackageViewModel>(packageObj);
            }
            return null;
        }

        public async Task<bool> UpdatePackageAsync(int packageId, UpdatePackageViewModel package)
        {
            var existingPackage = await _unitOfWork.PackageRepository.GetByIdAsync(packageId);
            if (existingPackage == null || existingPackage.IsDeleted == true)
            {
                return false;
            }

            _mapper.Map(package, existingPackage);
            _unitOfWork.PackageRepository.Update(existingPackage);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> DeletePackageAsync(int packageId)
        {
            var package = await _unitOfWork.PackageRepository.GetByIdAsync(packageId);
            if (package == null || package.IsDeleted == true)
            {
                return false;
            }

            package.IsDeleted = true;
            _unitOfWork.PackageRepository.Update(package);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

    }
}
