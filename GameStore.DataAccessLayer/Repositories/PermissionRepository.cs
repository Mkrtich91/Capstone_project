using GameStore.DataAccessLayer.Database;
using GameStore.DataAccessLayer.Interfaces.Entities;
using GameStore.DataAccessLayer.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
namespace GameStore.DataAccessLayer.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly DataContext _dbContext;

        public PermissionRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _dbContext.Permissions.ToListAsync();
        }

        public async Task<Permission> PermissionByNameAsync(string permissionName)
        {
            return await _dbContext.Permissions
                .FirstOrDefaultAsync(p => p.Name == permissionName);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByRoleAsync(string roleName)
        {
            return await _dbContext.Permissions
                                 .Where(p => p.RolePermissions.Any(rp => rp.Role.Name == roleName))
                                 .ToListAsync();
        }

        public async Task<Permission> GetPermissionByIdAsync(Guid permissionId)
        {
            return await _dbContext.Permissions
                .Include(p => p.RolePermissions)
                .Where(p => p.Id == permissionId)
                .FirstOrDefaultAsync();
        }
    }
}
