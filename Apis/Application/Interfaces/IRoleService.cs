using Application.ViewModels.RoleViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetRolesAsync();
        Task<RoleViewModel?> GetRoleByIdAsync(int roleId);
        Task<RoleViewModel?> CreateRoleAsync(CreateRoleViewModel role);
        Task<bool> UpdateRoleAsync(int roleId, UpdateRoleViewModel role);
        Task<bool> DeleteRoleAsync(int roleId);
    }
}
