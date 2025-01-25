using AutoMapper;
using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.Exceptions;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using MongoDB.Repositories.IRepository;
using MongoDB.Services.IServices;

namespace MongoDB.Services.Services
{
    public class OrderMongoDBService : IOrderMongoDBService
    {
        private readonly IOrderMongoDBRepository _repository;
        private readonly IMapper _mapper;

        public OrderMongoDBService(IOrderMongoDBRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetOrderResponse> GetOrderByIdAsync(string orderId)
        {
            if (!int.TryParse(orderId, out var parsedOrderId))
            {
                throw new NotFoundException($"Invalid order ID format: {orderId}. Must be a valid integer.");
            }

            var order = await _repository.GetOrderByIdAsync(parsedOrderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with ID '{orderId}' not found.");
            }

            return _mapper.Map<GetOrderResponse>(order);
        }


        public async Task<IEnumerable<GetOrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _repository.GetAllOrdersAsync();
            return orders.Select(order => _mapper.Map<GetOrderResponse>(order)).ToList();
        }
    }
}
