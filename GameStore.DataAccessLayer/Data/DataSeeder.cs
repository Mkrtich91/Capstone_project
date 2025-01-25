// <copyright file="DataSeeder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.DataAccessLayer.Data
{
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using Microsoft.EntityFrameworkCore;

    public class DataSeeder
    {
        public static void SeedGenres(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = Guid.NewGuid(), Name = "Action" },
                new Genre { Id = Guid.NewGuid(), Name = "Adventure" },
                new Genre { Id = Guid.NewGuid(), Name = "RPG" },
                new Genre { Id = Guid.NewGuid(), Name = "Strategy" },
                new Genre { Id = Guid.NewGuid(), Name = "Sports" });
        }

        public static void SeedPlatforms(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Platform>().HasData(
                new Platform { Id = Guid.NewGuid(), Type = "PC" },
                new Platform { Id = Guid.NewGuid(), Type = "PlayStation" },
                new Platform { Id = Guid.NewGuid(), Type = "Xbox" },
                new Platform { Id = Guid.NewGuid(), Type = "Nintendo Switch" });
        }

        public static void SeedPublishers(ModelBuilder modelBuilder)
        {
            var ubisoftId = Guid.NewGuid();
            var eaId = Guid.NewGuid();
            var nintendoId = Guid.NewGuid();

            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Id = ubisoftId, CompanyName = "Ubisoft", HomePage = "https://www.ubisoft.com", Description = "A leading publisher in video games." },
                new Publisher { Id = eaId, CompanyName = "EA", HomePage = "https://www.ea.com", Description = "Popular for sports games." },
                new Publisher { Id = nintendoId, CompanyName = "Nintendo", HomePage = "https://www.nintendo.com", Description = "Famous for the Mario franchise." });
        }
    }
}
