// <copyright file="Seed.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore
{
    using GameStore.DataAccessLayer.Database;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public class Seed
    {
        private readonly DataContext _dbContext;

        public Seed(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedDataContext()
        {
            SeedGenres();
            SeedPlatforms();
            SeedGames();
        }

        private void SeedGenres()
        {
            var genres = new List<Genre>
        {
            new Genre { Id = Guid.NewGuid(), Name = "Strategy" },
            new Genre { Id = Guid.NewGuid(), Name = "RPG" },
            new Genre { Id = Guid.NewGuid(), Name = "Sports" },
            new Genre { Id = Guid.NewGuid(), Name = "Races" },
            new Genre { Id = Guid.NewGuid(), Name = "Action" },
            new Genre { Id = Guid.NewGuid(), Name = "Adventure" },
            new Genre { Id = Guid.NewGuid(), Name = "Puzzle & Skill" },
        };

            var rtsId = genres[0].Id;
            var tbsId = genres[0].Id;
            var rallyId = genres[3].Id;
            var arcadeId = genres[3].Id;
            var formulaId = genres[3].Id;
            var offRoadId = genres[3].Id;
            var fpsId = genres[4].Id;
            var tpsId = genres[4].Id;

            genres.AddRange(new List<Genre>
        {
            new Genre { Id = Guid.NewGuid(), Name = "RTS", ParentGenreId = rtsId },
            new Genre { Id = Guid.NewGuid(), Name = "TBS", ParentGenreId = tbsId },
            new Genre { Id = Guid.NewGuid(), Name = "Rally", ParentGenreId = rallyId },
            new Genre { Id = Guid.NewGuid(), Name = "Arcade", ParentGenreId = arcadeId },
            new Genre { Id = Guid.NewGuid(), Name = "Formula", ParentGenreId = formulaId },
            new Genre { Id = Guid.NewGuid(), Name = "Off-road", ParentGenreId = offRoadId },
            new Genre { Id = Guid.NewGuid(), Name = "FPS", ParentGenreId = fpsId },
            new Genre { Id = Guid.NewGuid(), Name = "TPS", ParentGenreId = tpsId },
        });

            _dbContext.Genres.AddRange(genres);
            _dbContext.SaveChanges();
        }

        private void SeedPlatforms()
        {
            var platforms = new List<Platform>
        {
            new Platform { Id = Guid.NewGuid(), Type = "Mobile" },
            new Platform { Id = Guid.NewGuid(), Type = "Browser" },
            new Platform { Id = Guid.NewGuid(), Type = "Desktop" },
            new Platform { Id = Guid.NewGuid(), Type = "Console" },
        };

            _dbContext.Platforms.AddRange(platforms);
            _dbContext.SaveChanges();
        }

        private void SeedGames()
        {
            var genres = _dbContext.Genres.ToList();
            var platforms = _dbContext.Platforms.ToList();

            var games = new List<Game>
        {
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "FIFA 22",
                Key = "FIFA22",
                Description = "This is a simulation game based on football.",
                GameGenres = genres.Where(g => g.Name == "Sports").Select(g => new GameGenre { GenreId = g.Id }).ToList(),
                GamePlatforms = platforms.Where(p => p.Type == "Console" || p.Type == "Desktop" || p.Type == "Mobile").Select(p => new GamePlatform { PlatformId = p.Id }).ToList(),
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Cyberpunk 2077",
                Key = "CP2077",
                Description = "This is an open world, action-adventure story set in Night City.",
                GameGenres = genres.Where(g => g.Name == "Action").Select(g => new GameGenre { GenreId = g.Id }).ToList(),
                GamePlatforms = platforms.Where(p => p.Type == "Console" || p.Type == "Desktop").Select(p => new GamePlatform { PlatformId = p.Id }).ToList(),
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Cyberpunk 2078",
                Key = "CP2078",
                Description = "This is an open world, action-adventure story set in Night City78.",
                GameGenres = genres.Where(g => g.Name == "Action").Select(g => new GameGenre { GenreId = g.Id }).ToList(),
                GamePlatforms = platforms.Where(p => p.Type == "Console" || p.Type == "Desktop").Select(p => new GamePlatform { PlatformId = p.Id }).ToList(),
            },
        };

            _dbContext.Games.AddRange(games);
            _dbContext.SaveChanges();
        }
    }
}
