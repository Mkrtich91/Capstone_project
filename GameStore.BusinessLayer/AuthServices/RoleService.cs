using AutoMapper;
using GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO;
using GameStore.BusinessLayer.Interfaces.AuthDTOs.ResponseDTO;
using GameStore.BusinessLayer.Interfaces.AuthDTOs.WrappedDTOs;
using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.Exceptions;
using GameStore.BusinessLayer.Interfaces.IAuthServices;
using GameStore.DataAccessLayer.Interfaces.Entities;
using GameStore.DataAccessLayer.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameStore.BusinessLayer.AuthServices
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(RoleManager<UserRole> roleManager, IMapper mapper, ILogger<RoleService> logger, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return _mapper.Map<List<RoleDto>>(roles);
        }

        public async Task<RoleDto> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role == null)
            {
                throw new NotFoundException($"Role with id {id} was not found.");
            }

            return _mapper.Map<RoleDto>(role);
        }

        public async Task DeleteRoleByIdAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role == null)
            {
                throw new NotFoundException($"Role with id {id} was not found.");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Failed to delete the role.");
            }
        }

        public async Task<IEnumerable<string>> GetPermissionsAsync()
        {
            var permissions = await _unitOfWork.PermissionRepository.GetAllPermissionsAsync();
            return permissions.Select(p => p.Name);
        }

        public async Task<IEnumerable<string>> GetRolePermissionsAsync(Guid roleId)
        {
            var role = await _unitOfWork.RoleRepository.GetRolePermissionsAsync(roleId);

            if (role == null || !role.Any())
            {
                throw new NotFoundException($"Role with ID '{roleId}' not found.");
            }

            return role;
        }

        public async Task<RoleResponseDto> CreateRoleAsync(CreateRoleRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Role?.Name))
            {
                throw new ArgumentException("Role data is invalid or incomplete.");
            }

            if (await _roleManager.RoleExistsAsync(request.Role.Name))
            {
                throw new InvalidOperationException($"Role '{request.Role.Name}' already exists.");
            }

            var userRole = new UserRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Role.Name,
            };

            foreach (var permissionName in request.Permissions)
            {
                var permission = await _unitOfWork.PermissionRepository.PermissionByNameAsync(permissionName);
                if (permission == null)
                {
                    throw new NotFoundException($"Permission '{permissionName}' not found.");
                }

                userRole.RolePermissions.Add(new RolePermission
                {
                    PermissionId = permission.Id,
                    RoleId = userRole.Id,
                });
            }

            var createRoleResult = await _roleManager.CreateAsync(userRole);
            if (!createRoleResult.Succeeded)
            {
                var errors = string.Join(", ", createRoleResult.Errors.Select(e => e.Description));
                throw new Exception($"An error occurred while creating the role: {errors}");
            }

            _logger.LogInformation("Role '{RoleName}' created successfully with ID '{RoleId}'.", userRole.Name, userRole.Id);

            return new RoleResponseDto
            {
                Id = Guid.Parse(userRole.Id),
                Name = userRole.Name,
            };
        }

        public async Task<RoleResponseDto> UpdateRoleAsync(RoleDtoWrapper role)
        {
            try
            {
                _logger.LogInformation("UpdateRoleAsync: Updating role.");

                var userRole = await _roleManager.FindByIdAsync(role.Role.Id.ToString());
                if (userRole == null)
                {
                    throw new NotFoundException("Role not found.");
                }

                if (userRole.Name != role.Role.Name)
                {
                    userRole.Name = role.Role.Name;
                }

                var existingRolePermissions = await _unitOfWork.RoleRepository.GetRolePermissionsAsync(Guid.Parse(userRole.Id));

                if (existingRolePermissions.Any())
                {
                    var rolePermissionsToRemove = new List<RolePermission>();

                    foreach (var permissionName in existingRolePermissions)
                    {
                        var permission = await _unitOfWork.PermissionRepository.PermissionByNameAsync(permissionName);

                        if (permission != null)
                        {
                            rolePermissionsToRemove.Add(new RolePermission
                            {
                                PermissionId = permission.Id,
                                RoleId = userRole.Id,
                            });
                        }
                    }

                    await _unitOfWork.RoleRepository.RemoveRolePermissionsAsync(rolePermissionsToRemove);
                    await _unitOfWork.SaveAsync();
                }

                foreach (var permissionName in role.Permissions)
                {
                    var permission = await _unitOfWork.PermissionRepository.PermissionByNameAsync(permissionName);
                    if (permission == null)
                    {
                        throw new ArgumentException($"Permission '{permissionName}' not found.");
                    }

                    userRole.RolePermissions.Add(new RolePermission
                    {
                        PermissionId = permission.Id,
                        RoleId = userRole.Id,
                    });
                }

                var updateRoleResult = await _roleManager.UpdateAsync(userRole);
                if (!updateRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", updateRoleResult.Errors.Select(e => e.Description));
                    throw new Exception($"An error occurred while updating the role: {errors}");
                }

                _logger.LogInformation("UpdateRoleAsync: Role updated successfully.");

                return new RoleResponseDto
                {
                    Id = Guid.Parse(userRole.Id),
                    Name = userRole.Name,
                };
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("UpdateRoleAsync: ArgumentException - {Message}", ex.Message);
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateRoleAsync: Exception - {Message}", ex.Message);
                throw new Exception("An error occurred while updating the role: " + ex.Message);
            }
        }
    }
}