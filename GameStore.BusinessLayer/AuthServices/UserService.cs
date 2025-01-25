namespace GameStore.BusinessLayer.AuthServices
{
    using AutoMapper;
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO;
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.IAuthServices;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<IdentityUser> userManager, RoleManager<UserRole> roleManager, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new NotFoundException($"User with id {id} was not found.");
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteUserByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new NotFoundException($"User with id {id} was not found.");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("An error occurred while deleting the user.");
            }
        }

        public async Task<bool> AddUserAsync(UserCreateRequestDto request)
        {
            var existingUser = await _userManager.FindByNameAsync(request.User.Name);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"A user with the username '{request.User.Name}' already exists.");
            }

            var user = new IdentityUser { UserName = request.User.Name, Email = request.User.Name };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return false;
            }

            foreach (var roleId in request.Roles)
            {
                var role = await _roleManager.FindByIdAsync(roleId.ToString());

                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else
                {
                    throw new NotFoundException($"Role with the specified ID was not found.");
                }
            }

            return true;
        }

        public async Task<bool> UpdateUserAsync(UserUpdateRequestDto userRequest)
        {
            var user = await _userManager.FindByIdAsync(userRequest.User.Id.ToString());

            if (user == null)
            {
                throw new NotFoundException($"User with {userRequest.User.Id} not found.");
            }

            var originalUserName = user.UserName;
            var originalPasswordHash = user.PasswordHash;
            var originalRoles = await _userManager.GetRolesAsync(user);

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                user.UserName = userRequest.User.Name;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return false;
                }

                if (!string.IsNullOrEmpty(userRequest.Password))
                {
                    var passwordResetResult = await _userManager.RemovePasswordAsync(user);
                    if (!passwordResetResult.Succeeded)
                    {
                        user.UserName = originalUserName;
                        await _userManager.UpdateAsync(user);
                        await _unitOfWork.RollbackTransactionAsync();
                        return false;
                    }

                    var passwordAddResult = await _userManager.AddPasswordAsync(user, userRequest.Password);
                    if (!passwordAddResult.Succeeded)
                    {
                        user.PasswordHash = originalPasswordHash;
                        user.UserName = originalUserName;
                        await _userManager.UpdateAsync(user);
                        await _unitOfWork.RollbackTransactionAsync();
                        return false;
                    }
                }

                var rolesToRemove = originalRoles.Except(userRequest.Roles.Select(roleId => _roleManager.FindByIdAsync(roleId.ToString()).Result.Name)).ToList();
                var rolesToAdd = userRequest.Roles.Select(roleId => _roleManager.FindByIdAsync(roleId.ToString()).Result.Name).Except(originalRoles).ToList();

                if (rolesToRemove.Any())
                {
                    var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!removeRolesResult.Succeeded)
                    {
                        user.PasswordHash = originalPasswordHash;
                        user.UserName = originalUserName;
                        await _userManager.UpdateAsync(user);
                        await _unitOfWork.RollbackTransactionAsync();
                        return false;
                    }
                }

                if (rolesToAdd.Any())
                {
                    foreach (var roleName in rolesToAdd)
                    {
                        var addRoleResult = await _userManager.AddToRoleAsync(user, roleName);
                        if (!addRoleResult.Succeeded)
                        {
                            user.PasswordHash = originalPasswordHash;
                            user.UserName = originalUserName;
                            await _userManager.UpdateAsync(user);
                            await _userManager.RemoveFromRolesAsync(user, rolesToAdd);
                            await _unitOfWork.RollbackTransactionAsync();
                            return false;
                        }
                    }
                }

                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                user.UserName = originalUserName;
                user.PasswordHash = originalPasswordHash;
                await _userManager.UpdateAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRequest.Roles.Select(roleId => _roleManager.FindByIdAsync(roleId.ToString()).Result.Name));
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
        }

        public async Task<IEnumerable<RoleDto>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var roleEntities = await _roleManager.Roles
                .Where(r => roles.Contains(r.Name))
                .ToListAsync();

            var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roleEntities);

            return roleDtos;
        }
    }
}
