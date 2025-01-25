using GameStore.DataAccessLayer.Database;
using GameStore.DataAccessLayer.Interfaces.Entities;
using GameStore.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DataAccessLayer.Tests
{
    public class PermissionRepositoryTests
    {
        private readonly DbContextOptions<DataContext> _options;

        public PermissionRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{nameof(PermissionRepositoryTests)}_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task GetAllPermissionsAsync_ThreePermissions_ReturnsAllPermissions()
        {
            var expectedPermissions = new List<Permission>
        {
            new Permission { Id = Guid.NewGuid(), Name = "Permission 1" },
            new Permission { Id = Guid.NewGuid(), Name = "Permission 2" },
            new Permission { Id = Guid.NewGuid(), Name = "Permission 3" },
        };

            using (var context = new DataContext(_options))
            {
                context.Permissions.AddRange(expectedPermissions);
                await context.SaveChangesAsync();
            }

            IEnumerable<Permission> result;

            using (var context = new DataContext(_options))
            {
                var repository = new PermissionRepository(context);
                result = await repository.GetAllPermissionsAsync();
            }

            Assert.NotNull(result);
            Assert.Equal(expectedPermissions.Count, result.Count());
            Assert.Contains(result, p => p.Name == "Permission 1");
            Assert.Contains(result, p => p.Name == "Permission 2");
            Assert.Contains(result, p => p.Name == "Permission 3");

            using (var context = new DataContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task GetAllPermissionsAsync_EmptyDatabase_ReturnsEmptyList()
        {
            using (var context = new DataContext(_options))
            {
                context.Database.EnsureDeleted();
                await context.SaveChangesAsync();
            }

            IEnumerable<Permission> result;

            using (var context = new DataContext(_options))
            {
                var repository = new PermissionRepository(context);
                result = await repository.GetAllPermissionsAsync();
            }

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task PermissionByNameAsync_ValidPermission_ReturnsCorrectPermission()
        {
            var permission = new Permission { Id = Guid.NewGuid(), Name = "Test Permission" };

            using (var context = new DataContext(_options))
            {
                context.Permissions.Add(permission);
                await context.SaveChangesAsync();
            }

            Permission result;

            using (var context = new DataContext(_options))
            {
                var repository = new PermissionRepository(context);
                result = await repository.PermissionByNameAsync("Test Permission");
            }

            Assert.NotNull(result);
            Assert.Equal(permission.Name, result.Name);

            using (var context = new DataContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task GetPermissionsByRoleAsync_ValidRole_ReturnsExpectedPermissions()
        {
            var roleId = Guid.NewGuid().ToString();
            var role = new UserRole { Id = roleId, Name = "Admin" };

            var permission1 = new Permission { Id = Guid.NewGuid(), Name = "Read" };
            var permission2 = new Permission { Id = Guid.NewGuid(), Name = "Write" };

            var rolePermission1 = new RolePermission { RoleId = roleId.ToString(), PermissionId = permission1.Id };
            var rolePermission2 = new RolePermission { RoleId = roleId.ToString(), PermissionId = permission2.Id };

            using (var context = new DataContext(_options))
            {
                context.Roles.Add(role);
                context.Permissions.AddRange(permission1, permission2);
                context.RolePermissions.AddRange(rolePermission1, rolePermission2);
                await context.SaveChangesAsync();
            }

            IEnumerable<Permission> result;
            using (var context = new DataContext(_options))
            {
                var repository = new PermissionRepository(context);
                result = await repository.GetPermissionsByRoleAsync("Admin");
            }

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "Read");
            Assert.Contains(result, p => p.Name == "Write");
        }

        [Fact]
        public async Task PermissionByNameAsync_InvalidName_ReturnsNull()
        {
            using (var context = new DataContext(_options))
            {
                context.Database.EnsureDeleted();
                await context.SaveChangesAsync();
            }

            Permission result;

            using (var context = new DataContext(_options))
            {
                var repository = new PermissionRepository(context);
                result = await repository.PermissionByNameAsync("NonExistent Permission");
            }

            Assert.Null(result);
        }

        [Fact]
        public async Task GetPermissionsByRoleAsync_InvalidRole_ReturnsEmptyList()
        {
            var roleId = Guid.NewGuid();
            var role = new UserRole { Id = roleId.ToString(), Name = "Admin" };

            var permission = new Permission { Id = Guid.NewGuid(), Name = "Read" };
            var rolePermission = new RolePermission { RoleId = roleId.ToString(), PermissionId = permission.Id };

            using (var context = new DataContext(_options))
            {
                context.Roles.Add(role);
                context.Permissions.Add(permission);
                context.RolePermissions.Add(rolePermission);
                await context.SaveChangesAsync();
            }

            IEnumerable<Permission> result;
            using (var context = new DataContext(_options))
            {
                var repository = new PermissionRepository(context);
                result = await repository.GetPermissionsByRoleAsync("NonExistentRole");
            }

            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
