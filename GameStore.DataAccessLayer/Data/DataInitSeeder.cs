// <copyright file="DataInitSeeder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.DataAccessLayer.Data
{
    using System.Security.Claims;
    using GameStore.DataAccessLayer.Database;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using Microsoft.AspNetCore.Identity;

    public class DataInitSeeder
    {
        public static async Task SeedDataAsync(UserManager<IdentityUser> userManager, RoleManager<UserRole> roleManager, DataContext context)
        {
            await SeedRolesAsync(roleManager);

            await SeedPermissionsAsync(context);

            await SeedRolePermissionsAsync(roleManager, context);

            await SeedUsersAsync(userManager);

            await SeedRoleClaimsAsync(roleManager, context);

            await context.SaveChangesAsync();
        }

        private static async Task SeedRolesAsync(RoleManager<UserRole> roleManager)
        {
            var roles = new List<string>
        {
            "Admin", "Manager", "Moderator", "User", "Guest",
        };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new UserRole { Name = role });
                }
            }
        }

        private static async Task SeedPermissionsAsync(DataContext context)
        {
            var permissions = new List<Permission>
        {
            new Permission { Name = "ManageSystemUsers" },
            new Permission { Name = "SeeDeletedGames" },
            new Permission { Name = "ManageCommentsForDeletedGames" },
            new Permission { Name = "EditDeletedGame" },
            new Permission { Name = "EditOrders" },
            new Permission { Name = "ViewOrdersHistory" },
            new Permission { Name = "ChangeOrderStatusToShipped" },
            new Permission { Name = "ManageGameComments" },
            new Permission { Name = "BanUsersFromCommenting" },
            new Permission { Name = "SeeGamesInStock" },
            new Permission { Name = "CommentOnGames" },
            new Permission { Name = "ReadOnlyAccess" },
            new Permission { Name = "AddGame" },
            new Permission { Name = "DeleteGame" },
            new Permission { Name = "ViewGame" },
            new Permission { Name = "UpdateGame" },
            new Permission { Name = "DeleteUser" },
            new Permission { Name = "AddUser" },
            new Permission { Name = "UpdateUser" },
            new Permission { Name = "ManageGameComments" },
            new Permission { Name = "CanBuyGame" },
            new Permission { Name = "DeleteComment" },
            new Permission { Name = "ManageSystemRoles" },
            new Permission { Name = "DeleteRole" },
            new Permission { Name = "ManageSystemPermissions" },
            new Permission { Name = "CreateRole" },
            new Permission { Name = "UpdateRole" },
            new Permission { Name = "AddGenre" },
            new Permission { Name = "UpdateGenre" },
            new Permission { Name = "DeleteGenre" },
            new Permission { Name = "DeleteFromCart" },
            new Permission { Name = "DeleteOrderGame" },
            new Permission { Name = "UpdateOrderDetail" },
            new Permission { Name = "AddPlatform" },
            new Permission { Name = "UpdatePlatform" },
            new Permission { Name = "DeletePlatform" },
            new Permission { Name = "AddPublisher" },
            new Permission { Name = "UpdatePlatform" },
            new Permission { Name = "UpdatePlatform" },
        };

            foreach (var permission in permissions)
            {
                if (!context.Permissions.Any(p => p.Name == permission.Name))
                {
                    context.Permissions.Add(permission);
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedRolePermissionsAsync(RoleManager<UserRole> roleManager, DataContext context)
        {
            var rolePermissions = new Dictionary<string, List<string>>
        {
            {
                "Admin", new List<string>
                {
                    "ManageSystemUsers", "ManageSystemRoles", "SeeDeletedGames", "ManageCommentsForDeletedGames", "EditDeletedGame",
                    "AddGame", "DeleteGame", "ViewGame", "UpdateGame", "DeleteUser", "ManageGameComments", "BanUsersFromCommenting",
                    "AddUser", "UpdateUser", "CanBuyGame", "DeleteComment", "DeleteRole", "CreateRole", "UpdateRole", "ManageSystemPermissions",
                    "AddGenre", "UpdateGenre", "DeleteGenre", "DeleteFromCart", "ViewOrdersHistory", "DeleteOrderGame", "UpdateOrderDetail",
                    "ChangeOrderStatusToShipped", "AddPlatform", "UpdatePlatform", "DeletePlatform", "AddPublisher", "UpdatePublisher",
                }
            },
            {
                "Manager", new List<string>
                {
                    "EditOrders", "ViewOrdersHistory", "ChangeOrderStatusToShipped",
                    "AddGame", "DeleteGame", "ViewGame", "UpdateGame", "ManageGameComments", "BanUsersFromCommenting",
                    "CanBuyGame", "DeleteComment", "AddGenre", "UpdateGenre", "DeleteGenre", "DeleteFromCart", "UpdateOrderDetail",
                    "AddPlatform", "UpdatePlatform", "DeletePlatform","AddPublisher", "UpdatePublisher",
                }
            },
            {
                    "Moderator", new List<string>
                {
                    "ManageGameComments", "BanUsersFromCommenting","ViewGame", "CanBuyGame", "DeleteComment", "DeleteFromCart",
                }
            },
            {
                    "User", new List<string>
                {
                    "SeeGamesInStock", "CommentOnGames", "ViewGame","ManageGameComments","CanBuyGame", "DeleteFromCart",
                }
            },
            {
                "Guest", new List<string>
                {
                    "ReadOnlyAccess",
                }
            },
        };

            foreach (var rolePermission in rolePermissions)
            {
                var role = await roleManager.FindByNameAsync(rolePermission.Key);
                if (role == null)
                {
                    continue;
                }

                foreach (var permissionName in rolePermission.Value)
                {
                    var permission = context.Permissions.FirstOrDefault(p => p.Name == permissionName);
                    if (permission == null)
                    {
                        continue;
                    }

                    if (!context.RolePermissions.Any(rp => rp.RoleId == role.Id && rp.PermissionId == permission.Id))
                    {
                        context.RolePermissions.Add(new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = permission.Id,
                        });
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedUsersAsync(UserManager<IdentityUser> userManager)
        {
            var users = new List<(string username, string email, string password)>
        {
            ("admin", "admin@admin.com", "Admin123!"),
            ("manager", "manager@manager.com", "Manager123!"),
            ("moderator", "moderator@moderator.com", "Moderator123!"),
            ("user", "user@user.com", "User123!"),
            ("guest", "guest@guest.com", "Guest123!"),
        };

            foreach (var (username, email, password) in users)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new IdentityUser { UserName = username, Email = email };
                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        Console.WriteLine($"User {username} created.");
                    }
                    else
                    {
                        Console.WriteLine($"Error creating user {username}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    Console.WriteLine($"User {username} already exists.");
                }
            }
        }

        private static async Task SeedRoleClaimsAsync(RoleManager<UserRole> roleManager, DataContext context)
        {
            var roleClaims = new Dictionary<string, List<IdentityRoleClaim<string>>>
        {
            { "Admin", new List<IdentityRoleClaim<string>>
                {
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ManageSystemUsers" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "SeeDeletedGames" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ManageDeletedGameComments" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "EditDeletedGames" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeleteUser" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ManageGameComments" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "BanUsersFromCommenting" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ViewGame" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "AddUser" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "UpdateUser" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "CanBuyGame" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeleteComment" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ManageSystemRoles" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeleteRole" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ManageSystemPermissions" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "CreateRole" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "UpdateRole" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "AddGenre" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "UpdateGenre" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeleteGenre" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeleteFromCart" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ViewOrdersHistory" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeleteOrderGame" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "UpdateOrderDetail" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ChangeOrderStatusToShipped" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "AddPlatform" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "UpdatePlatform" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeletePlatform" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "AddPublisher" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "UpdatePublisher" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeletePublisher" },
                }
            },
            {
                "Manager", new List<IdentityRoleClaim<string>>
                {
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ManageBusinessEntities" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "EditOrders" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ViewOrdersHistory" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ChangeOrderStatusToShipped" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ManageGameComments" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "BanUsersFromCommenting" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ViewGame" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "CanBuyGame" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeleteComment" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "AddGenre" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "UpdateGenre" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeleteGenre" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeleteFromCart" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "UpdateOrderDetail" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "AddPlatform" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "UpdatePlatform" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeletePlatform" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "AddPublisher" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "UpdatePublisher" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeletePublisher" },
                }
            },
            { "Moderator", new List<IdentityRoleClaim<string>>
                {
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ManageGameComments" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "BanUsersFromCommenting" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ViewGame" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "CanBuyGame" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeleteComment" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeleteFromCart" },
                }
            },
            {
                "User", new List<IdentityRoleClaim<string>>
                {
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "SeeGamesInStock" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ManageGameComments" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ViewGame" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "CanBuyGame" },
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "DeleteFromCart" },
                }
            },
            {
                "Guest", new List<IdentityRoleClaim<string>>
                {
                    new IdentityRoleClaim<string> { ClaimType = "Permission", ClaimValue = "ReadOnlyAccess" },
                }
            },
        };

            foreach (var roleClaim in roleClaims)
            {
                var role = await roleManager.FindByNameAsync(roleClaim.Key);
                if (role == null)
                {
                    role = new UserRole { Name = roleClaim.Key };
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        continue;
                    }
                }

                foreach (var roleClaimItem in roleClaim.Value)
                {
                    var claim = new Claim(roleClaimItem.ClaimType, roleClaimItem.ClaimValue);
                    var roleClaimsExist = await roleManager.GetClaimsAsync(role);
                    if (!roleClaimsExist.Any(c => c.Type == claim.Type && c.Value == claim.Value))
                    {
                        await roleManager.AddClaimAsync(role, claim);
                    }
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
