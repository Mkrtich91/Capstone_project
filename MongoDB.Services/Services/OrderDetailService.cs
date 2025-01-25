using AutoMapper;
using GameStore.BusinessLayer.Interfaces.Exceptions;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using MongoDB.Entities.Converter;
using MongoDB.Entities.Entities;
using MongoDB.Repositories.IRepository;
using MongoDB.Services.IServices;
using System.Diagnostics.Metrics;

namespace MongoDB.Services.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetOrderGameResponse>> GetAllOrderDetailsAsync()
        {
            var orderDetails = await _orderDetailRepository.GetAllOrdersAsync();
            return _mapper.Map<IEnumerable<GetOrderGameResponse>>(orderDetails);
        }

        public async Task<IEnumerable<GetOrderGameResponse>> GetOrderDetailsByOrderIdAsync(string orderId)
        {
            if (!int.TryParse(orderId, out var parsedOrderId))
            {
                throw new NotFoundException($"No order details found for Order ID: {orderId}");
            }

            var orderDetails = await _orderDetailRepository.FindByOrderIdAsync(parsedOrderId);

            if (orderDetails == null || !orderDetails.Any())
            {
                throw new NotFoundException($"No order details found for Order ID: {parsedOrderId}");
            }

            return _mapper.Map<IEnumerable<GetOrderGameResponse>>(orderDetails);
        }

    }
}
