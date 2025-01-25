namespace GameStore.Controllers
{
    using GameStore.BusinessLayer.AuthServices;
    using GameStore.BusinessLayer.Interfaces.DataProvider;
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.IFacade;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("games")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IPublisherService _publisherService;
        private readonly IOrderService _orderService;
        private readonly ICommentService _commentService;
        private readonly IGamesFacade _gamesFacade;

        public GameController(IGameService gameService, IPublisherService publisherService, BusinessLayer.Interfaces.Services.IOrderService orderService, ICommentService commentService, IGamesFacade gamesFacade)
        {
            _gameService = gameService;
            _publisherService = publisherService;
            _orderService = orderService;
            _commentService = commentService;
            _gamesFacade = gamesFacade;

        }

        [PermissionName("AddGame")]
        [HttpPost]
        public async Task<IActionResult> AddGame([FromBody] CreateGameRequest request)
        {
            var addGame = await _gameService.AddGameAsync(request);
            return Ok($"Game with ID '{addGame.Id}' has been added successfully.");
        }

        [AllowAnonymous]
        [HttpGet("{key}")]
        public async Task<IActionResult> GetGameByKey(string key)
        {
            var game = await _gameService.GetGameByKeyAsync(key);
            return Ok(game);
        }

        [AllowAnonymous]
        [HttpGet("find/{id}")]
        public async Task<IActionResult> GetGameById(string id)
        {
            var result = await _gamesFacade.GetGameByIdAsync(id);
            return Ok(result);
        }

        [PermissionName("UpdateGame")]
        [HttpPut]
        public async Task<IActionResult> UpdateGame([FromBody] GameUpdateRequest request)
        {
            var game = await _gameService.UpdateGameAsync(request);
            return Ok($"Game with ID {game.Id} updated successfully.");
        }

        [PermissionName("DeleteGame")]
        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteGame(string key)
        {

            await _gameService.DeleteGameByKeyAsync(key);
            return Ok("Game deleted successfully.");
        }

        [Authorize]
        [HttpGet("{key}/file")]
        public async Task<IActionResult> DownloadGameFile(string key)
        {
            var fileBytes = await _gameService.DownloadGameFileAsync(key);

            var fileName = $"{key}.json";
            var contentType = "application/octet-stream";

            var stream = new MemoryStream(fileBytes);

            return File(stream, contentType, fileName);
        }

        [AllowAnonymous]
        [HttpGet("GetAllGames")]
        public async Task<IActionResult> GetAllGames()
        {
            var result = await _gamesFacade.GetAllGamesAsync();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{key}/genres")]
        public async Task<IActionResult> GetGenresByGameKey(string key)
        {
            var genres = await _gameService.GetGenresByGameKeyAsync(key);
            return Ok(genres);
        }

        [AllowAnonymous]
        [HttpGet("{key}/platforms")]
        public async Task<IActionResult> GetPlatformsByGameKey(string key)
        {
            var platformDtos = await _gameService.GetPlatformsByGameKeyAsync(key);
            return Ok(platformDtos);
        }

        [AllowAnonymous]
        [HttpGet("TotalGamesCount")]
        public async Task<IActionResult> GetTotalGamesCount()
        {
            var totalGamesCount = await _gameService.GetTotalGamesCountAsync();
            var message = $"Total Games Count: {totalGamesCount}";
            return Ok(new { Message = message });
        }

        [AllowAnonymous]
        [HttpGet("{key}/publisher")]
        public async Task<IActionResult> GetPublisherByGameKey(string key)
        {
            var publisherResponse = await _publisherService.GetPublisherByGameKeyAsync(key);

            return Ok(publisherResponse);
        }

        [PermissionName("CanBuyGame")]
        [HttpPost("{key}/buy")]
        public async Task<IActionResult> AddGameToCart(string key)
        {
            await _orderService.AddGameToOrderAsync(key);
            return Ok("Game added to cart successfully.");
        }

        [PermissionName("ManageGameComments")]
        [HttpPost("{key}/comments")]
        public async Task<IActionResult> AddComment(string key, [FromBody] CreateCommentRequest request)
        {
            var comment = await _commentService.AddCommentAsync(request, key);
            return CreatedAtAction(nameof(AddComment), new { key }, comment);
        }

        [AllowAnonymous]
        [HttpGet("{key}/comments")]
        public async Task<IActionResult> GetAllCommentsByGameKey(string key)
        {
            var comments = await _commentService.GetAllCommentsByGameKeyAsync(key);
            return Ok(comments);
        }

        [PermissionName("DeleteComment")]
        [HttpDelete("{key}/comments/{id}")]
        public async Task<IActionResult> DeleteComment(string key, Guid id)
        {
            await _commentService.DeleteCommentAsync(key, id);
            return Ok("Comment deleted successfully.");
        }

        [AllowAnonymous]
        [HttpGet("pagination-options")]
        public ActionResult<IQueryable<string>> GetPaginationOptions()
        {
            var options = _gameService.GetPaginationOptions();
            return Ok(options);
        }

        [AllowAnonymous]
        [HttpGet("sorting-options")]
        public ActionResult<IQueryable<string>> GetSortingOptions()
        {
            var options = _gameService.GetSortingOptions();
            return Ok(options);
        }

        [AllowAnonymous]
        [HttpGet("publish-date-filter-options")]
        public ActionResult<IQueryable<string>> GetPublishDateFilterOptions()
        {
            var options = _gameService.GetPublishDateFilterOptions();
            return Ok(options);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetGames([FromQuery] GameQueryDto queryDto)
        {
            var response = await _gameService.GetFilteredSortedPaginatedGamesAsync(queryDto);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetFetchAllGames()
        {
            var games = await _gamesFacade.GetAllGameAndProductOverviewsAsync();
            return Ok(games);
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetGamesByFiltersAsync([FromQuery] string name, [FromQuery] string genre, [FromQuery] Guid? platformId, [FromQuery] double? minPrice, [FromQuery] double? maxPrice)
        {
            var games = await _gameService.GetGamesByFiltersAsync(name, genre, platformId, minPrice, maxPrice);
            return Ok(games);
        }
    }
}
