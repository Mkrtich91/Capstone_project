namespace GameStore.Controllers
{
    using GameStore.BusinessLayer.AuthServices;
    using GameStore.BusinessLayer.Facade;
    using GameStore.BusinessLayer.Interfaces.AuthDTOs.RequestDTO;
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.IFacade;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MongoDB.Services.IServices;

    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentProcessingService _paymentProcessingService;
        private readonly IPaymentService _paymentService;
        private readonly IOrderFacade _orderFacade;
        private readonly IOrderGameFacade _orderGamesFacade;
        private readonly IOrderGameService _orderGameService;

        public OrdersController(
            IOrderService orderService,
            IPaymentProcessingService paymentProcessingService,
            IPaymentService paymentService,
            IOrderFacade orderFacade,
            IOrderGameFacade orderGamesFacade,
            IOrderGameService orderGameService
            )
        {
            _orderService = orderService;
            _paymentProcessingService = paymentProcessingService;
            _paymentService = paymentService;
            _orderFacade = orderFacade;
            _orderGamesFacade = orderGamesFacade;
            _orderGameService = orderGameService;
        }

        [PermissionName("DeleteFromCart")]
        [HttpDelete("cart/{key}")]
        public async Task<IActionResult> DeleteGameFromCart(string key)
        {
            await _orderService.RemoveGameFromOrderAsync(key);
            return Ok("Game removed from cart successfully.");
        }

        [PermissionName("ViewOrdersHistory")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetPaidAndCancelledOrders()
        {
            var orders = await _orderService.GetPaidAndCancelledOrdersAsync();
            return Ok(orders);
        }

        [PermissionName("ViewOrdersHistory")]
        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderFacade.GetAllOrdersAsync();
            return Ok(orders);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(string id)
        {
            var order = await _orderFacade.GetOrderByIdAsync(id);
            return Ok(order);
        }

        [AllowAnonymous]
        [HttpGet("GetAllOrderGamesDetails")]
        public async Task<IActionResult> GetAllOrderGamesAndDetails()
        {
            var result = await _orderGamesFacade.GetAllOrderDetailsAsync();
            return Ok(result);
        }

        [PermissionName("ViewOrdersHistory")]
        [HttpGet("games-details/{orderId}")]
        public async Task<IActionResult> GetOrderGameDetails(string orderId)
        {
            var orderDetails = await _orderGamesFacade.GetOrderDetailsByOrderIdAsync(orderId);
            return Ok(orderDetails);
        }

        [AllowAnonymous]
        [HttpGet("cart")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCart()
        {
            var cartItems = await _orderService.GetCartAsync();

            return Ok(cartItems);
        }

        [AllowAnonymous]
        [HttpGet("payment-methods")]
        public IActionResult GetPaymentMethods()
        {
            var paymentMethods = _paymentService.GetPaymentMethods();
            return Ok(new { paymentMethods });
        }

        [PermissionName("CanBuyGame")]
        [HttpPost("payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest paymentRequest)
        {
            var result = await _paymentProcessingService.ProcessPaymentAsync(paymentRequest);
            return result;
        }

        [PermissionName("UpdateOrderDetail")]
        [HttpPatch("details/{id}/quantity")]
        public async Task<IActionResult> UpdateOrderDetailQuantityAsync(string id, [FromBody] UpdateQuantityRequest request)
        {
            await _orderGamesFacade.UpdateOrderDetailQuantityAsync(id, request.Count);

            return Ok(new { message = "Order detail quantity updated successfully." });
        }

        [PermissionName("DeleteOrderGame")]
        [HttpDelete("details/{id}")]
        public async Task<IActionResult> DeleteOrderDetailAsync(string id)
        {
            await _orderGamesFacade.RemoveOrderGameAndDetailsByOrderIdAsync(id);

            return Ok(new { message = $"Order game with ID {id} has been successfully removed from the order." });
        }

        [PermissionName("ChangeOrderStatusToShipped")]
        [HttpPost("{id}/ship")]
        public async Task<IActionResult> ShipOrderAsync(Guid id)
        {
            await _orderService.ShipOrderAsync(id);
            return Ok(new { Message = "Order has been shipped." });
        }

        [PermissionName("UpdateOrderDetail")]
        [HttpPost("{id}/details/{key}")]
        public async Task<IActionResult> AddGameToOrderDetail(Guid id, string key, [FromBody] AddGameRequest request)
        {

            await _orderGameService.AddGameToOrderByIdAsync(id, key, request.Quantity);

            return Ok(new { message = "Game added to order details successfully." });
        }
    }
}