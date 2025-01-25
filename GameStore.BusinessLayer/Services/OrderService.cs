// <copyright file="OrderService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Services
{
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;
    using Microsoft.Extensions.Logging;

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderGameService _orderGameService;
        private readonly IGameRepository _gameRepository;
        private static readonly Guid StubCustomerId = Guid.Parse("7dce8347-4181-4316-9210-302361340975");
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IOrderGameService orderGameService, IGameRepository gameRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _orderGameService = orderGameService;
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public async Task AddGameToOrderAsync(string gameKey)
        {
            var game = await _gameRepository.GetGameByKeyAsync(gameKey);
            if (game == null)
            {
                throw new NotFoundException("Game not found.");
            }

            var order = await _orderRepository.GetOpenOrderByCustomerIdAsync(StubCustomerId);
            if (order == null)
            {
                order = new Order
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    CustomerId = StubCustomerId,
                    Status = OrderStatus.Open,
                };
                Console.WriteLine($"Creating new order for customer {StubCustomerId}");
                await _orderRepository.AddOrderAsync(order);
            }
            else
            {
                Console.WriteLine($"Found existing open order {order.Id} for customer {StubCustomerId}");
            }

            await _orderGameService.AddGameToOrderAsync(order.Id, game);
        }

        public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException("Order not found.");
            }

            switch (order.Status)
            {
                case OrderStatus.Open:
                    if (newStatus == OrderStatus.Paid || newStatus == OrderStatus.Cancelled)
                    {
                        order.Status = newStatus;
                        await _orderRepository.UpdateOrderAsync(order);
                        Console.WriteLine($"Order {orderId} status updated to {order.Status}");
                    }
                    else
                    {
                        throw new InvalidOperationException("Invalid status transition.");
                    }
                    break;

                case OrderStatus.Paid:
                    throw new InvalidOperationException("Cannot change the status of a paid order.");

                case OrderStatus.Cancelled:
                    throw new InvalidOperationException("Cannot change the status of a cancelled order.");

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task RemoveGameFromOrderAsync(string gameKey)
        {
            var game = await _gameRepository.GetGameByKeyAsync(gameKey);
            if (game == null)
            {
                throw new NotFoundException("Game not found.");
            }

            var order = await _orderRepository.GetOpenOrderByCustomerIdAsync(StubCustomerId);
            if (order == null)
            {
                throw new NotFoundException("Open order not found for the customer.");
            }

            var orderGame = await _orderGameService.GetOrderGameByOrderIdAndGameIdAsync(order.Id, game.Id);
            if (orderGame == null)
            {
                throw new NotFoundException("Game not found in the order.");
            }

            await _orderGameService.RemoveGameFromOrderAsync(order.Id, game.Id);
        }

        public async Task<IEnumerable<GetOrderResponse>> GetPaidAndCancelledOrdersAsync()
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(OrderStatus.Paid, OrderStatus.Cancelled);
            return orders.Select(o => new GetOrderResponse
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                Date = o.Date ?? DateTime.MinValue,
            });
        }

        public async Task<GetOrderResponse> GetOrderByIdAsync(string orderId)
        {
            if (!Guid.TryParse(orderId, out var parsedOrderId))
            {
                throw new NotFoundException($"Invalid order ID format: {orderId}. Must be a valid GUID.");
            }

            var order = await _orderRepository.GetOrderByIdAsync(parsedOrderId);
            if (order == null)
            {
                throw new NotFoundException("Order not found.");
            }

            return new GetOrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                Date = order.Date ?? DateTime.MinValue,
            };
        }

        public async Task<IEnumerable<CartItemDto>> GetCartAsync()
        {
            var order = await _orderRepository.GetOpenOrderByCustomerIdAsync(StubCustomerId);

            if (order == null)
            {
                order = new Order
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.UtcNow,
                    CustomerId = StubCustomerId,
                    Status = OrderStatus.Open,
                };
                Console.WriteLine($"Creating new cart order for customer {StubCustomerId}");
                await _orderRepository.AddOrderAsync(order);
                return Enumerable.Empty<CartItemDto>();
            }

            return order.OrderGames.Select(od => new CartItemDto
            {
                GameId = od.GameId,
                Price = od.Price,
                Quantity = od.Quantity,
                Discount = od.Discount.GetValueOrDefault(),
            }).ToList();
        }

        public async Task<CartDetailsDto> GetCartDetailsAsync(Guid customerId)
{
    var order = await _orderRepository.GetOpenOrderByCustomerIdAsync(customerId);

    if (order == null)
        {
           throw new NotFoundException("No open order found.");
        }

    var cartDetails = new CartDetailsDto
            {
                OrderId = order.Id,
                Items = order.OrderGames.Select(og => new CartItemDto
                {
                    GameId = og.GameId,
                    Price = og.Price,
                    Quantity = og.Quantity,
                    Discount = og.Discount.GetValueOrDefault(),
                }).ToList(),
            };
    return cartDetails;
}

        public async Task ShipOrderAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                _logger.LogError($"Order {orderId} not found.");
                throw new NotFoundException("Order not found.");
            }

            if (order.Status != OrderStatus.Paid)
            {
                _logger.LogWarning($"Order {orderId} is not paid. Current status: {order.Status}");
                throw new InvalidOperationException("Order must be paid before it can be shipped.");
            }

            order.Status = OrderStatus.Shipped;

            await _orderRepository.UpdateOrderAsync(order);

            _logger.LogInformation($"Order {orderId} has been shipped.");
        }
    }
}
