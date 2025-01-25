namespace GameStore.BusinessLayer.Facade
{
    using GameStore.BusinessLayer.Interfaces.BaseClass;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.IFacade;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using MongoDB.Services.IServices;

    public class OrderFacade : FacadeBase, IOrderFacade
    {
        private readonly IOrderService _sqlOrderService;
        private readonly IOrderMongoDBService _mongoOrderService;

        public OrderFacade(IOrderService sqlOrderService, IOrderMongoDBService mongoOrderService)
        {
            _sqlOrderService = sqlOrderService;
            _mongoOrderService = mongoOrderService;
        }

        public async Task<GetOrderResponse> GetOrderByIdAsync(string orderId)
        {
            return
                await GetResponseOrNull(() => _sqlOrderService.GetOrderByIdAsync(orderId)) ??
                await GetResponseOrNull(() => _mongoOrderService.GetOrderByIdAsync(orderId)) ??
                throw new NotFoundException($"Order with ID '{orderId}' not found.");
        }

        public async Task<IEnumerable<GetOrderResponse>> GetAllOrdersAsync()
        {
            var sqlOrdersTask = _sqlOrderService.GetPaidAndCancelledOrdersAsync();
            var mongoOrdersTask = _mongoOrderService.GetAllOrdersAsync();

            await Task.WhenAll(sqlOrdersTask, mongoOrdersTask);

            var sqlOrders = await sqlOrdersTask;
            var mongoOrders = await mongoOrdersTask;

            return sqlOrders.Concat(mongoOrders);
        }
    }
}
