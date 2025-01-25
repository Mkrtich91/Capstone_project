namespace GameStore.BusinessLayer.Facade
{
    using GameStore.BusinessLayer.Interfaces.BaseClass;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.IFacade;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using MongoDB.Services.IServices;

    public class GenreFacade : FacadeBase, IGenreFacade
    {
        private readonly ICategoryService _categoryService;
        private readonly IGenreService _genreService;

        public GenreFacade(ICategoryService categoryService, IGenreService genreService)
        {
            _categoryService = categoryService;
            _genreService = genreService;
        }

        public async Task<IEnumerable<GetGenreResponse>> GetGenresAndCategoriesAsync()
        {
            var genresTask = _genreService.GetAllGenresAsync();
            var categoriesTask = _categoryService.GetAllGenresAsync();

            await Task.WhenAll(genresTask, categoriesTask);

            var genres = await genresTask;
            var categories = await categoriesTask;

            return genres.Concat(categories);
        }

        public async Task<GetGenreResponse> GetGenreOrCategoryByIdAsync(string id)
        {
            return
                await GetResponseOrNull(() => _genreService.GetGenreByIdAsync(id)) ??
                await GetResponseOrNull(() => _categoryService.GetGenreByIdAsync(id)) ??
                throw new NotFoundException($"Genre or Category with ID {id} not found.");
        }
    }
}
