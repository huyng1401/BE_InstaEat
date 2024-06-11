using Application.Interfaces;
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
        public async Task<List<RoleViewModel>> GetRolesAsync()
        {
            var roles = await _unitOfWork.RoleRepository.GetAllNotDeletedAsync();
            var result = _mapper.Map<List<RoleViewModel>>(roles);
            return result;
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
            var roleObj = _mapper.Map<Role>(role);
            await _unitOfWork.RoleRepository.AddAsync(roleObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync() > 0;
            if (isSuccess)
            {
                return _mapper.Map<RoleViewModel>(roleObj);
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
