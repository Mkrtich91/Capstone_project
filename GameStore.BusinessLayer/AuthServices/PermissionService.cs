using GameStore.BusinessLayer.Interfaces.IAuthServices;
using GameStore.DataAccessLayer.Interfaces.Repositories;
namespace GameStore.BusinessLayer.AuthServices
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<string>> GetPermissionsByRolesAsync(IEnumerable<string> roles)
        {
            if (roles == null || !roles.Any())
            {
                throw new ArgumentException("Roles cannot be null or empty.");
            }

            var allPermissions = new HashSet<string>();

            foreach (var role in roles)
            {
                var roleDetails = await _unitOfWork.RoleRepository.GetRoleWithDetailsByName(role);
                if (roleDetails?.RolePermissions != null)
                {
                    foreach (var rolePermission in roleDetails.RolePermissions)
                    {
                        if (!string.IsNullOrEmpty(rolePermission.Permission?.Name))
                        {
                            allPermissions.Add(rolePermission.Permission.Name);
                        }
                    }
                }
            }

            return allPermissions;
        }
    }
}
