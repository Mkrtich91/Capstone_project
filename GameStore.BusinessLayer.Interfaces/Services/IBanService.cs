// <copyright file="IBanService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.Services
{
    public interface IBanService
    {
        void BanUser(string userName, string duration);

        bool IsUserBanned(string userName);

        IEnumerable<string> GetBanDurations();
    }
}
