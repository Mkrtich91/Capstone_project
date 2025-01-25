// <copyright file="RoleClaims.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.DataAccessLayer.Data
{
    public static class RoleClaims
    {
        public const string ReadOnlyAccess = "ReadOnlyAccess";
        public const string ManageBusinessEntities = "ManageBusinessEntities";
        public const string ManageUsersAndRoles = "ManageUsersAndRoles";
        public const string ViewOrdersHistory = "ViewOrdersHistory";
        public const string EditOrders = "EditOrders";
        public const string ManageOrders = "ManageOrders";
        public const string ManageNotifications = "ManageNotifications";
    }
}
