namespace GameStore.BusinessLayer.Services
{
    using GameStore.DataAccessLayer.Database;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using Microsoft.EntityFrameworkCore;

    public class GameCatalogService
    {
        private readonly DataContext _context;

        public GameCatalogService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Game>> GetGamesByFiltersAsync(
            string genre = null,
            string platform = null,
            string publisher = null,
            double? minPrice = null,
            double? maxPrice = null,
            int? minStock = null,
            int? maxStock = null,
            DateTime? minPublishedDate = null,
            DateTime? maxPublishedDate = null,
            int page = 1,
            int pageSize = 20)
        {
            var query = _context.Games
                .Include(g => g.GameGenres)
                .Include(g => g.GamePlatforms)
                .Include(g => g.Publisher)
                .AsQueryable();

            if (!string.IsNullOrEmpty(genre))
            {
                query = query.Where(g => g.GameGenres.Any(gg => gg.Genre.Name.Equals(genre, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrEmpty(platform))
            {
                query = query.Where(g => g.GamePlatforms.Any(gp => gp.Platform.Type.Equals(platform, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrEmpty(publisher))
            {
                query = query.Where(g => g.Publisher.CompanyName.Equals(publisher, StringComparison.OrdinalIgnoreCase));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(g => g.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(g => g.Price <= maxPrice.Value);
            }

            if (minStock.HasValue)
            {
                query = query.Where(g => g.UnitInStock >= minStock.Value);
            }

            if (maxStock.HasValue)
            {
                query = query.Where(g => g.UnitInStock <= maxStock.Value);
            }

            if (minPublishedDate.HasValue)
            {
                query = query.Where(g => g.PublishedDate >= minPublishedDate.Value);
            }

            if (maxPublishedDate.HasValue)
            {
                query = query.Where(g => g.PublishedDate <= maxPublishedDate.Value);
            }

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }
    }
}
