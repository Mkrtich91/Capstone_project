using GameStore.BusinessLayer.Interfaces.Exceptions;
using GameStore.BusinessLayer.Interfaces.BaseClass;
using GameStore.BusinessLayer.Interfaces.IFacade;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using GameStore.BusinessLayer.Interfaces.Services;
using MongoDB.Services.IServices;
using GameStore.BusinessLayer.Interfaces.DTO;
using Microsoft.Extensions.Logging;

namespace GameStore.BusinessLayer.Facade
{
    public class GamesFacade : FacadeBase, IGamesFacade
    {
        private readonly IGameService _gameService;
        private readonly IProductService _productService;
        private readonly ILogger<FacadeBase> _logger;
        public GamesFacade(IGameService gameService, IProductService productService,ILogger<FacadeBase> logger)
        {
            _gameService = gameService;
            _productService = productService;
            _logger = logger;
        }

        public async Task<IEnumerable<GetGameResponse>> GetAllGamesAsync()
        {
            var gamesTask = _gameService.GetAllGamesAsync();
            var productsTask = _productService.GetAllProductsAsync();

            await Task.WhenAll(gamesTask, productsTask);

            var games = await gamesTask;
            var products = await productsTask;

            return products.Concat(games);
        }

        public async Task<GetGameResponse> GetGameByIdAsync(string id)
        {
            return
                await GetResponseOrNull(() => _gameService.GetGameByIdAsync(id)) ??
                await GetResponseOrNull(() => _productService.FindProductByIdAsync(id)) ??
                throw new NotFoundException($"Game or Product with ID {id} not found.");
        }

        public async Task<IEnumerable<GameOverviewDto>> GetAllGameAndProductOverviewsAsync()
        {
            var gameOverviews = await _gameService.FetchAllGamesAsync();

            var productOverviews = await _productService.GetAllProductOverviewsAsync();

            var combinedOverviews = gameOverviews.Concat(productOverviews);

            return combinedOverviews;
        }
    }
}
