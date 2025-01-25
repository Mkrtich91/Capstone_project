using GameStore.DataAccessLayer.Database;
using GameStore.DataAccessLayer.Interfaces.Entities;
using GameStore.DataAccessLayer.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DataAccessLayer.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _dbContext;
        private readonly DbSet<UserRole> _dbSet;

        public RoleRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<UserRole>();
        }

        public async Task<UserRole> GetRoleWithDetailsByName(string id)
        {
            return await _dbSet
                .Include(role => role.RolePermissions)
                    .ThenInclude(pr => pr.Permission)
                .FirstOrDefaultAsync(role => role.Name == id);
        }

        public async Task<IEnumerable<string>> GetRolePermissionsAsync(Guid roleId)
        {
            var permissions = await _dbContext.RolePermissions
                .Where(rp => rp.RoleId == roleId.ToString())
                .Include(rp => rp.Permission)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();

            return permissions;
        }

        public async Task RemoveRolePermissionsAsync(IEnumerable<RolePermission> rolePermissions)
        {
            await _dbContext.RolePermissions
                .Where(rp => rolePermissions.Select(rp => rp.PermissionId).Contains(rp.PermissionId) && rp.RoleId == rolePermissions.First().RoleId)
                .ForEachAsync(rp => _dbContext.RolePermissions.Remove(rp));
        }
    }
}