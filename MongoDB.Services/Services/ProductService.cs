using AutoMapper;
using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.Exceptions;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using MongoDB.Repositories.IRepository;
using MongoDB.Services.IServices;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace MongoDB.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetGameResponse>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllAsync();
            foreach (var product in products)
            {
                product.ViewCount++;
                await _repository.UpdateAsync(product);
            }
            var productResponses = _mapper.Map<IEnumerable<GetGameResponse>>(products);
            return productResponses;
        }
        public async Task<GetGameResponse> FindProductByIdAsync(string productId)
        {
            if (!int.TryParse(productId, out int parsedProductId))
            {
                throw new NotFoundException("Invalid product ID format.");
            }

            var product = await _repository.FindByProductIdAsync(parsedProductId);
            if (product != null)
            {
                product.ViewCount++;
                await _repository.UpdateAsync(product);
                return _mapper.Map<GetGameResponse>(product);
            }

            throw new NotFoundException($"Product with ID {productId} not found.");
        }

        public async Task<IEnumerable<GameOverviewDto>> GetAllProductOverviewsAsync()
        {
            var products = await _repository.GetAllAsync();

            var productDtos = _mapper.Map<IEnumerable<GameOverviewDto>>(products);

            return productDtos;
        }

    }
}
