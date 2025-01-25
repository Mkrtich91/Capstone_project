// <copyright file="GuidConverter.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace GameStore.BusinessLayer.Interfaces.Converters
{
    public static class GuidConverter
    {
        public static Guid? ConvertToGuid(string userId)
        {
            return Guid.TryParse(userId, out var guid) ? guid : (Guid?)null;
        }
    }
}
