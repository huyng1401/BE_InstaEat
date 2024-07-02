using Application.Commons;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels.RoleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<RoleViewModel>>> GetRoles([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var roles = await _roleService.GetRolesAsync(pageIndex, pageSize);
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleViewModel>> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound("Role not found.");
            }
            return Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult<RoleViewModel>> CreateRole([FromBody] CreateRoleViewModel createRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = await _roleService.CreateRoleAsync(createRoleViewModel);
            if (role == null)
            {
                return BadRequest("Unable to create role.");
            }

            return CreatedAtAction(nameof(GetRoleById), new { id = role.RoleId }, role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleViewModel updateRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdated = await _roleService.UpdateRoleAsync(id, updateRoleViewModel);
            if (!isUpdated)
            {
                return NotFound("Role not found.");
            }

            return Ok("Successfully Updated!!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var isDeleted = await _roleService.DeleteRoleAsync(id);
            if (!isDeleted)
            {
                return NotFound("Role not found.");
            }

            return Ok("Successfully Deleted!!");
        }
    }
}
