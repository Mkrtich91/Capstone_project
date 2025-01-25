// <copyright file="PermissionListProvider.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.DataAccessLayer.Data
{
    public static class PermissionListProvider
    {
        public static Dictionary<string, string> GetPermissionList()
        {
            return new Dictionary<string, string>
        {
            { "Games", RoleClaims.ReadOnlyAccess },
            { "Game", RoleClaims.ReadOnlyAccess },
            { "UpdateGame", RoleClaims.ManageBusinessEntities },
            { "DeleteGame", RoleClaims.ManageBusinessEntities },
            { "AddGame", RoleClaims.ManageBusinessEntities },
            { "Users", RoleClaims.ReadOnlyAccess },
            { "User", RoleClaims.ReadOnlyAccess },
            { "UpdateUser", RoleClaims.ManageBusinessEntities },
            { "DeleteUser", RoleClaims.ManageBusinessEntities },
            { "AddUser", RoleClaims.ManageBusinessEntities },
            { "Roles", RoleClaims.ReadOnlyAccess },
            { "Role", RoleClaims.ReadOnlyAccess },
            { "UpdateRole", RoleClaims.ManageUsersAndRoles },
            { "DeleteRole", RoleClaims.ManageUsersAndRoles },
            { "AddRole", RoleClaims.ManageUsersAndRoles },
            { "Genres", RoleClaims.ReadOnlyAccess },
            { "Genre", RoleClaims.ReadOnlyAccess },
            { "UpdateGenre", RoleClaims.ManageBusinessEntities },
            { "DeleteGenre", RoleClaims.ManageBusinessEntities },
            { "AddGenre", RoleClaims.ManageBusinessEntities },
            { "Platforms", RoleClaims.ReadOnlyAccess },
            { "Platform", RoleClaims.ReadOnlyAccess },
            { "UpdatePlatform", RoleClaims.ManageBusinessEntities },
            { "DeletePlatform", RoleClaims.ManageBusinessEntities },
            { "AddPlatform", RoleClaims.ManageBusinessEntities },
            { "Publishers", RoleClaims.ReadOnlyAccess },
            { "Publisher", RoleClaims.ReadOnlyAccess },
            { "UpdatePublisher", RoleClaims.ManageBusinessEntities },
            { "DeletePublisher", RoleClaims.ManageBusinessEntities },
            { "AddPublisher", RoleClaims.ManageBusinessEntities },
            { "Order", RoleClaims.ReadOnlyAccess },
            { "Orders", RoleClaims.ReadOnlyAccess },
            { "History", RoleClaims.ViewOrdersHistory },
            { "Basket", RoleClaims.ManageBusinessEntities },
            { "MakeOrder", RoleClaims.ManageBusinessEntities },
            { "UpdateOrder", RoleClaims.EditOrders },
            { "ShipOrder", RoleClaims.EditOrders },
            { "Notifications", RoleClaims.ReadOnlyAccess },
        };
        }
    }
}
