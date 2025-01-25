using GameStore.BusinessLayer.Interfaces.Exceptions;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using GameStore.BusinessLayer.Interfaces.Services;
using GameStore.DataAccessLayer.Interfaces.Entities;
using GameStore.DataAccessLayer.Interfaces.Repositories;
using MongoDB.Entities.Converter;

namespace GameStore.BusinessLayer.Services
{
    public class OrderGameService : IOrderGameService
    {
        private readonly IOrderGameRepository _orderGameRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderGameService(IOrderGameRepository orderGameRepository, IGameRepository gameRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderGameRepository = orderGameRepository;
            _gameRepository = gameRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddGameToOrderAsync(Guid orderId, Game game)
        {
            var existingOrderGame = await _orderGameRepository.GetOrderGameByOrderIdAndGameIdAsync(orderId, game.Id);

            if (1 > game.UnitInStock)
            {
                throw new InvalidOperationException("Cannot add the game to the cart because it is out of stock.");
            }

            if (existingOrderGame != null)
            {
                existingOrderGame.Quantity++;
                await _orderGameRepository.UpdateOrderGameAsync(existingOrderGame);
            }
            else
            {
                var orderGame = new OrderGame
                {
                    OrderId = orderId,
                    GameId = game.Id,
                    Price = game.Price,
                    Quantity = 1,
                    Discount = game.Discount,
                };
                await _orderGameRepository.AddOrderGameAsync(orderGame);
            }

            game.UnitInStock -= 1;
            await _gameRepository.UpdateGameAsync(game);
        }

        public async Task<OrderGame> GetOrderGameByOrderIdAndGameIdAsync(Guid orderId, Guid gameId)
        {
            return await _orderGameRepository.GetOrderGameByOrderIdAndGameIdAsync(orderId, gameId);
        }

        public async Task RemoveGameFromOrderAsync(Guid orderId, Guid gameId)
        {
            var orderGame = await _orderGameRepository.GetOrderGameByOrderIdAndGameIdAsync(orderId, gameId);
            if (orderGame == null)
            {
                throw new NotFoundException("Game not found in the order.");
            }

            await _orderGameRepository.RemoveOrderGameAsync(orderGame);
        }

        public async Task<IEnumerable<GetOrderGameResponse>> GetOrderDetailsByOrderIdAsync(string orderId)
        {
            if (!Guid.TryParse(orderId, out var parsedOrderId))
            {
                throw new NotFoundException($"Order with ID {orderId} not found.");
            }

            var order = await _orderRepository.GetOrderByIdAsync(parsedOrderId);
            if (order == null)
            {
                throw new NotFoundException($"Order with ID {orderId} not found.");
            }

            return order.OrderGames.Select(og => new GetOrderGameResponse
            {
                GameId = og.GameId,
                Price = og.Price,
                Quantity = og.Quantity,
                Discount = og.Discount,
            }).ToList();
        }

        public async Task<IEnumerable<GetOrderGameResponse>> GetAllOrderGamesAsync(bool includeRecent = false)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-30);

            var orderGames = await _orderGameRepository.GetAllOrderGamesAsync();

            var orderGamesQuery = orderGames
                .Where(og => og.Order.Date <= cutoffDate || includeRecent);

            var orderGameDtos = orderGamesQuery.Select(og => new GetOrderGameResponse
            {
                GameId = og.GameId,
                Price = og.Price,
                Quantity = og.Quantity,
                Discount = og.Discount,
            });

            return orderGameDtos.ToList();
        }

        public async Task UpdateGameQuantityInOrderAsync(string orderGameId, int count)
        {
            if (count < 0)
            {
                throw new InvalidOperationException($"Quantity cannot be less than 0. Provided count: {count}");
            }

            if (!Guid.TryParse(orderGameId, out var parsedOrderGameId))
            {
                throw new NotFoundException($"OrderGame with ID {orderGameId} not found.");
            }

            var orderGame = await _orderGameRepository.GetOrderGameByIdAsync(parsedOrderGameId);

            if (orderGame == null)
            {
                throw new NotFoundException($"OrderGame with ID {orderGameId} not found.");
            }

            orderGame.Quantity = count;

            await _orderGameRepository.UpdateOrderGameAsync(orderGame);
        }

        public async Task RemoveOrderGameByIdAsync(string orderGameId)
        {
            if (!Guid.TryParse(orderGameId, out var parsedOrderGameId))
            {
                throw new NotFoundException($"OrderGame with ID {orderGameId} not found.");
            }

            var orderGame = await _orderGameRepository.GetOrderGameByIdAsync(parsedOrderGameId);

            if (orderGame == null)
            {
                throw new NotFoundException($"OrderGame with ID {orderGameId} not found.");
            }

            await _orderGameRepository.RemoveOrderGameAsync(orderGame);
        }

        public async Task AddGameToOrderByIdAsync(Guid orderId, string gameKey, int quantity)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var order = await _unitOfWork.OrderRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    throw new NotFoundException($"Order with ID {orderId} not found.");
                }

                if (order.Status == OrderStatus.Paid || order.Status == OrderStatus.Cancelled)
                {
                    throw new InvalidOperationException("Cannot add a game to a paid or cancelled order.");
                }

                var game = await _unitOfWork.GameRepository.GetGameByKeyAsync(gameKey);
                if (game == null)
                {
                    throw new NotFoundException($"Game with key {gameKey} not found.");
                }

                if (game.UnitInStock < quantity)
                {
                    throw new InvalidOperationException("Cannot add the game to the cart because there is not enough stock.");
                }

                var existingOrderGame = await _unitOfWork.OrderGameRepository.GetOrderGameByOrderIdAndGameIdAsync(orderId, game.Id);

                if (existingOrderGame != null)
                {
                    existingOrderGame.Quantity += quantity;
                    await _unitOfWork.OrderGameRepository.UpdateOrderGameAsync(existingOrderGame);
                }
                else
                {
                    var orderGame = new OrderGame
                    {
                        OrderId = orderId,
                        GameId = game.Id,
                        Price = game.Price,
                        Quantity = quantity,
                        Discount = game.Discount,
                    };
                    await _unitOfWork.OrderGameRepository.AddOrderGameAsync(orderGame);
                }

                game.UnitInStock -= quantity;
                await _unitOfWork.GameRepository.UpdateGameAsync(game);

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
