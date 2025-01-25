using GameStore.DataAccessLayer.Interfaces.Entities;
namespace GameStore.DataAccessLayer.Interfaces.Repositories
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllPermissionsAsync();

        Task<Permission> PermissionByNameAsync(string permissionName);

        Task<IEnumerable<Permission>> GetPermissionsByRoleAsync(string roleName);

        Task<Permission> GetPermissionByIdAsync(Guid permissionId);
    }
}
