// <copyright file="AuthService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.AuthServices
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO;
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.ResponseDTO;
    using GameStore.BusinessLayer.Interfaces.IAuthServices;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AuthService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPermissionService _permissionService;

        public AuthService(
            IJwtTokenGenerator jwtTokenGenerator,
            UserManager<IdentityUser> userManager,
            ILogger<AuthService> logger,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IPermissionService permissionService)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userManager = userManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _permissionService = permissionService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            _logger.LogInformation("Login attempt for {Login}", loginRequestDto.Login);

            var user = await _userManager.FindByEmailAsync(loginRequestDto.Login);
            if (user == null)
            {
                _logger.LogWarning("User not found: {Login}", loginRequestDto.Login);
                throw new UnauthorizedAccessException("Invalid login.");
            }

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (!isValid)
            {
                _logger.LogWarning("Invalid password for user: {Login}", loginRequestDto.Login);
                throw new UnauthorizedAccessException("Invalid password.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var permissions = await _permissionService.GetPermissionsByRolesAsync(roles);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
    };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = await _jwtTokenGenerator.GenerateTokenAsync(user, roles, claims);

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Token generation failed for user: {Login}", loginRequestDto.Login);
                throw new InvalidOperationException("Token generation failed.");
            }

            _logger.LogInformation("Login successful for {Login}", loginRequestDto.Login);
            return new LoginResponseDto { Token = "Bearer " + token };
        }

        public async Task<IActionResult> HasAccessToPageAsync(AccessRequestDto request)
        {
            var userClaims = _httpContextAccessor.HttpContext.User.Claims;
            var userPermissions = userClaims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToList();

            var permission = await _unitOfWork.PermissionRepository
                .GetPermissionByIdAsync(Guid.Parse(request.TargetId));

            if (permission == null || permission.Name != request.TargetPage)
            {
                return new ObjectResult(new { Message = "Permission not found or page mismatch" })
                {
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

            if (userPermissions.Contains(permission.Name))
            {
                return new OkObjectResult(new { AccessGranted = true, Permission = permission.Name });
            }

            return new ObjectResult(new { Message = "Access denied", Reason = "User does not have the required permission." })
            {
                StatusCode = StatusCodes.Status403Forbidden,
            };
        }
    }
}
