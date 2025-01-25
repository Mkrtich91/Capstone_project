using GameStore.BusinessLayer.AuthServices;
using GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO;
using GameStore.BusinessLayer.Interfaces.AuthDTOs.WrappedDTOs;
using GameStore.BusinessLayer.Interfaces.IAuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UsersController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDtoWrapper requestWrapper)
        {
            var request = requestWrapper.Model;

            var loginResponse = await _authService.LoginAsync(request);

            return Ok(new { token = loginResponse.Token });
        }

        [Authorize]
        [HttpPost("access")]
        public async Task<IActionResult> CheckPageAccess([FromBody] AccessRequestDto request)
        {
            return await _authService.HasAccessToPageAsync(request);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [PermissionName("ManageSystemUsers")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [PermissionName("DeleteUser")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(Guid id)
        {
            await _userService.DeleteUserByIdAsync(id);

            return Ok(new { message = "User deleted successfully." });
        }

       // [PermissionName("AddUser")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserCreateRequestDto request)
        {
            await _userService.AddUserAsync(request);
            return Ok(new { message = "User created successfully." });
        }

        [PermissionName("UpdateUser")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequestDto userRequest)
        {
            await _userService.UpdateUserAsync(userRequest);

            return Ok(new { message = "User updated successfully." });
        }

        [PermissionName("ManageSystemUsers")]
        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserRolesAsync(string id)
        {
            var roles = await _userService.GetUserRolesAsync(id);
            return Ok(roles);
        }
    }
}
