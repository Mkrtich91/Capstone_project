using AutoMapper;
using GameStore.BusinessLayer.Interfaces.Exceptions;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using MongoDB.Repositories.IRepository;
using MongoDB.Services.IServices;
namespace MongoDB.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<List<GetGenreResponse>> GetAllGenresAsync()
        {
            var categories = await _repository.GetAllCategoriesAsync();
            return _mapper.Map<List<GetGenreResponse>>(categories);
        }

        public async Task<GetGenreResponse> GetGenreByIdAsync(string categoryId)
        {
            if (!int.TryParse(categoryId, out var parsedCategoryId))
            {
                throw new NotFoundException($"Invalid ID format: {categoryId}. Must be a valid integer or guid");
            }

            var category = await _repository.GetCategoryByIdAsync(parsedCategoryId);
            if (category != null)
            {
                return _mapper.Map<GetGenreResponse>(category);
            }

            throw new NotFoundException($"Category with ID {parsedCategoryId} not found.");
        }

    }
}
