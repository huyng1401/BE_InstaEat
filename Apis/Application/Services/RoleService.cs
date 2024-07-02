using Application.Commons;
using Application.Interfaces;
using Application.Utils;
using Application.ViewModels.RoleViewModels;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Pagination<RoleViewModel>> GetRolesAsync(int pageIndex = 0, int pageSize = 10)
        {
            var roles = await _unitOfWork.RoleRepository.GetAllNotDeletedAsync();
            var paginatedRoles = await ListPagination<RoleViewModel>.PaginateList(_mapper.Map<List<RoleViewModel>>(roles), pageIndex, pageSize);
            return paginatedRoles;
        }

        public async Task<RoleViewModel?> GetRoleByIdAsync(int roleId)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);
            if (role != null && role.IsDeleted == false)
            {
                return _mapper.Map<RoleViewModel>(role);
            }
            return null;
        }

        public async Task<RoleViewModel?> CreateRoleAsync(CreateRoleViewModel role)
        {
            var existingRole = await _unitOfWork.RoleRepository.GetAllNotDeletedAsync();
            if (existingRole.Any(r => r.RoleName == role.RoleName))
            {
                throw new ArgumentException("A role with the same name already exists.");
            }

            var roleObj = _mapper.Map<Role>(role);
            await _unitOfWork.RoleRepository.AddAsync(roleObj);
            try
            {
                var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
                if (isSuccess)
                {
                    return _mapper.Map<RoleViewModel>(roleObj);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                throw;
            }
            return null;
        }

        public async Task<bool> UpdateRoleAsync(int roleId, UpdateRoleViewModel role)
        {
            var existingRole = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);
            if (existingRole == null || existingRole.IsDeleted == true)
            {
                return false;
            }

            _mapper.Map(role, existingRole);
            _unitOfWork.RoleRepository.Update(existingRole);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);
            if (role == null || role.IsDeleted == true)
            {
                return false;
            }

            role.IsDeleted = true;
            _unitOfWork.RoleRepository.Update(role);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }
    }
}
