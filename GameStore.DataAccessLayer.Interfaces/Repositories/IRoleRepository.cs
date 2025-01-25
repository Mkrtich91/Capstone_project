using GameStore.DataAccessLayer.Interfaces.Entities;

namespace GameStore.DataAccessLayer.Interfaces.Repositories
{
    public interface IRoleRepository 
    {
        Task<UserRole> GetRoleWithDetailsByName(string id);

        Task<IEnumerable<string>> GetRolePermissionsAsync(Guid roleId);

        Task RemoveRolePermissionsAsync(IEnumerable<RolePermission> rolePermissions);
    }
}
