namespace GameStore.Controllers
{
    using GameStore.BusinessLayer.AuthServices;
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO;
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.WrappedDTOs;
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.IAuthServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [PermissionName("ManageSystemRoles")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRoleById(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            return Ok(role);
        }

        [PermissionName("DeleteRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleById(Guid id)
        {
            await _roleService.DeleteRoleByIdAsync(id);
            return Ok(new { message = $"Role with id {id} deleted successfully." });
        }

        [PermissionName("ManageSystemPermissions")]
        [HttpGet("permissions")]
        public async Task<IActionResult> GetPermissions()
        {
            var permissions = await _roleService.GetPermissionsAsync();
            return Ok(permissions);
        }

        [PermissionName("ManageSystemPermissions")]
        [HttpGet("{id}/permissions")]
        public async Task<IActionResult> GetRolePermissions(Guid id)
        {
            var permissions = await _roleService.GetRolePermissionsAsync(id);
            return Ok(permissions);
        }

        [PermissionName("CreateRole")]
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequestDto request)
        {
            var createdRole = await _roleService.CreateRoleAsync(request);

            return CreatedAtAction(nameof(CreateRole), new { id = createdRole.Id }, createdRole);
        }

        [PermissionName("UpdateRole")]
        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDtoWrapper roleWrapper)
        {
            var updatedRole = await _roleService.UpdateRoleAsync(roleWrapper);

            return Ok(updatedRole);
        }
    }
}
