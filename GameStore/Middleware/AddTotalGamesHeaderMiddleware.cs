namespace GameStore.Middleware
{
    using GameStore.BusinessLayer.Interfaces.Services;
    using Microsoft.Extensions.Caching.Memory;

    public class AddTotalGamesHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "TotalGamesCount";
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(1);

        public AddTotalGamesHeaderMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!_cache.TryGetValue(CacheKey, out int totalGamesCount))
            {
                var gameService = context.RequestServices.GetRequiredService<IGameService>();
                totalGamesCount = await gameService.GetTotalGamesCountAsync();

                _cache.Set(CacheKey, totalGamesCount, _cacheDuration);
            }

            context.Response.Headers.Add("x-total-numbers-of-games", totalGamesCount.ToString());

            await _next(context);
        }
    }
}
