using Bogus;
using GameStore.DataAccessLayer.Database;
using GameStore.DataAccessLayer.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DataAccessLayer.Data
{
    public class SeedGamesService
    {
        private readonly DataContext _context;

        public SeedGamesService(DataContext context)
        {
            _context = context;
        }

        public async Task SeedGamesAsync(int count = 100000)
        {
            var publishers = await _context.Publishers.ToListAsync();
            if (publishers.Count == 0)
            {
                var defaultPublisher = new Publisher
                {
                    CompanyName = "Default Publisher",
                    HomePage = "http://defaultpublisher.com",
                    Description = "Default Publisher Description"
                };
                await _context.Publishers.AddAsync(defaultPublisher);
                await _context.SaveChangesAsync();
                publishers.Add(defaultPublisher);
            }

            var gameFaker = new Faker<Game>()
                .RuleFor(g => g.Name, f => f.Commerce.ProductName())
                .RuleFor(g => g.Key, f => f.Random.AlphaNumeric(10))
                .RuleFor(g => g.Description, f => f.Lorem.Sentence(10))
                .RuleFor(g => g.Price, f => f.Random.Double(5, 100))
                .RuleFor(g => g.UnitInStock, f => f.Random.Int(1, 1000))
                .RuleFor(g => g.Discount, f => f.Random.Int(0, 50))
                .RuleFor(g => g.PublishedDate, f => f.Date.Past(5))
                .RuleFor(g => g.ViewCount, f => f.Random.Int(0, 10000))
                .RuleFor(g => g.PublisherId, f => f.PickRandom(publishers).Id);

        var games = gameFaker.Generate(count);

            await _context.Games.AddRangeAsync(games);
            await _context.SaveChangesAsync();
        }
    }

}
