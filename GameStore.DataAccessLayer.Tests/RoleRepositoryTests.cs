using GameStore.DataAccessLayer.Database;
using GameStore.DataAccessLayer.Interfaces.Entities;
using GameStore.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
namespace GameStore.DataAccessLayer.Tests
{
    public class RoleRepositoryTests
    {
        private readonly DbContextOptions<DataContext> _options;

        public RoleRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"RoleRepositoryTests_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task GetRoleWitDetailsByName_ValidRoleName_ReturnsRoleWithDetails()
        {
            var roleId = Guid.NewGuid().ToString();
            var roleName = "Admin";
            var role = new UserRole
            {
                Id = roleId,
                Name = roleName,
                RolePermissions = new List<RolePermission>
        {
            new RolePermission
            {
                RoleId = roleId,
                PermissionId = Guid.NewGuid(),
                Permission = new Permission { Id = Guid.NewGuid(), Name = "View" },
            },
        },
            };

            using (var context = new DataContext(_options))
            {
                context.UserRoles.Add(role);
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new RoleRepository(context);
                var result = await repository.GetRoleWithDetailsByName(roleName);

                Assert.NotNull(result);
                Assert.Equal(roleName, result.Name);
                Assert.Single(result.RolePermissions);
                Assert.Equal("View", result.RolePermissions.First().Permission.Name);
            }
        }

        [Fact]
        public async Task GetRoleWitDetailsByName_InvalidRoleName_ReturnsNull()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new RoleRepository(context);
                var result = await repository.GetRoleWithDetailsByName("NonExistentRole");

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetRolePermissionsAsync_ValidRoleId_ReturnsPermissions()
        {
            var roleId = Guid.NewGuid().ToString();
            var permissions = new List<RolePermission>
    {
        new RolePermission
        {
            RoleId = roleId,
            PermissionId = Guid.NewGuid(),
            Permission = new Permission { Id = Guid.NewGuid(), Name = "Edit" },
        },
        new RolePermission
        {
            RoleId = roleId,
            PermissionId = Guid.NewGuid(),
            Permission = new Permission { Id = Guid.NewGuid(), Name = "Delete" },
        },
    };

            using (var context = new DataContext(_options))
            {
                context.RolePermissions.AddRange(permissions);
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new RoleRepository(context);
                var result = await repository.GetRolePermissionsAsync(Guid.Parse(roleId));

                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
                Assert.Contains("Edit", result);
                Assert.Contains("Delete", result);
            }
        }

        [Fact]
        public async Task GetRolePermissionsAsync_InvalidRoleId_ReturnsEmptyList()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new RoleRepository(context);
                var result = await repository.GetRolePermissionsAsync(Guid.NewGuid());

                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task RemoveRolePermissionsAsync_ValidPermissions_RemovesFromDatabase()
        {
            var roleId = Guid.NewGuid().ToString();
            var permissions = new List<RolePermission>
    {
        new RolePermission { RoleId = roleId, PermissionId = Guid.NewGuid() },
        new RolePermission { RoleId = roleId, PermissionId = Guid.NewGuid() },
    };

            using (var context = new DataContext(_options))
            {
                context.RolePermissions.AddRange(permissions);
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new RoleRepository(context);
                await repository.RemoveRolePermissionsAsync(permissions);
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var remainingPermissions = await context.RolePermissions.Where(rp => rp.RoleId == roleId).ToListAsync();
                Assert.Empty(remainingPermissions);
            }
        }

        [Fact]
        public async Task RemoveRolePermissionsAsync_InvalidPermissions_NoChangesToDatabase()
        {
            var roleId = Guid.NewGuid().ToString();
            var permissionsToRemove = new List<RolePermission>
    {
        new RolePermission { RoleId = roleId, PermissionId = Guid.NewGuid() },
    };

            using (var context = new DataContext(_options))
            {
                var repository = new RoleRepository(context);
                await repository.RemoveRolePermissionsAsync(permissionsToRemove);
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var remainingPermissions = await context.RolePermissions.ToListAsync();
                Assert.Empty(remainingPermissions);
            }
        }
    }
}
