namespace GameStore.BusinessLayer.Facade
{
    using GameStore.BusinessLayer.Interfaces.BaseClass;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.IFacade;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using MongoDB.Services.IServices;

    public class OrderGameFacade : FacadeBase, IOrderGameFacade
    {
        private readonly IOrderGameService _orderGameService;
        private readonly IOrderDetailService _orderDetailService;

        public OrderGameFacade(
            IOrderGameService orderGameService,
            IOrderDetailService orderDetailService)
        {
            _orderGameService = orderGameService;
            _orderDetailService = orderDetailService;
        }

        public async Task<IEnumerable<GetOrderGameResponse>> GetOrderDetailsByOrderIdAsync(string orderId)
        {
            return
            await GetResponseOrNull(() => _orderGameService.GetOrderDetailsByOrderIdAsync(orderId)) ??
            await GetResponseOrNull(() => _orderDetailService.GetOrderDetailsByOrderIdAsync(orderId)) ??
            throw new NotFoundException($"Order with ID {orderId} not found in either service.");
        }

        public async Task<IEnumerable<GetOrderGameResponse>> GetAllOrderDetailsAsync()
        {
            var orderGames = await _orderGameService.GetAllOrderGamesAsync();
            var orderDetails = await _orderDetailService.GetAllOrderDetailsAsync();

            var combinedList = orderGames.Concat(orderDetails).ToList();

            return combinedList;
        }

        public async Task UpdateOrderDetailQuantityAsync(string orderDetailId, int newQuantity)
        {
            if (newQuantity < 0)
            {
                throw new InvalidOperationException($"Quantity cannot be less than 0. Provided count: {newQuantity}");
            }

            var result = await ExecuteActionOrNull(() => _orderGameService.UpdateGameQuantityInOrderAsync(orderDetailId, newQuantity));

            if (result == null)
            {
                throw new NotFoundException($"Unable to update order quantity for Order ID {orderDetailId}. Both services failed.");
            }
        }

        public async Task RemoveOrderGameAndDetailsByOrderIdAsync(string orderId)
        {
            var result = await ExecuteActionOrNull(() => _orderGameService.RemoveOrderGameByIdAsync(orderId));
            if (result == null)
            {
                throw new NotFoundException($"Unable to Delete OrderGame by {orderId} orderId. Both services failed.");
            }
        }
    }
}
