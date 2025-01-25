namespace GameStore.Controllers
{
    using GameStore.BusinessLayer.AuthServices;
    using GameStore.BusinessLayer.Interfaces.IFacade;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("genres")]
    public class GenreController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IGenreService _genreService;
        private readonly IGenreFacade _genreFacade;

        public GenreController(IGameService gameService, IGenreService genreService, IGenreFacade genreFacade)
        {
            _gameService = gameService;
            _genreService = genreService;
            _genreFacade= genreFacade;
        }

        [AllowAnonymous]
        [HttpGet("{id}/games")]
        public async Task<IActionResult> GetGamesByGenreId(Guid id)
        {
            var games = await _gameService.GetGamesByGenreIdAsync(id);
            return Ok(games);
        }

        [PermissionName("AddGenre")]
        [HttpPost]
        public async Task<IActionResult> AddGenre([FromBody] CreateGenreRequest request)
        {
            var genre = await _genreService.AddGenreAsync(request);
            return Ok($"Genre with ID {genre.Id} added successfully.");
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreOrCategoryById(string id)
        {
            var result = await _genreFacade.GetGenreOrCategoryByIdAsync(id);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetGenresAndCategories()
        {
            var result = await _genreFacade.GetGenresAndCategoriesAsync();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}/genres")]
        public async Task<IActionResult> GetgenresByParentId(Guid id)
        {
            var subgenres = await _genreService.GetgenresByParentIdAsync(id);

            return Ok(subgenres);
        }

        [PermissionName("UpdateGenre")]
        [HttpPut]
        public async Task<IActionResult> UpdateGenre(UpdateGenreRequest request)
        {
            var updatedGenre = await _genreService.UpdateGenreAsync(request.Genre);

            return Ok($"Genre with ID {updatedGenre.Id} updated successfully.");
        }

        [PermissionName("DeleteGenre")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(Guid id)
        {
            var deleted = await _genreService.DeleteGenreAsync(id);
            return Ok($"Genre with ID {deleted.Id} deleted successfully");
        }
    }
}
