using GameStore.DataAccessLayer.Database;
using GameStore.DataAccessLayer.Interfaces.Entities;
using GameStore.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DataAccessLayer.Tests
{
    public class OrderGameRepositoryTests
    {
        private readonly DbContextOptions<DataContext> _options;

        public OrderGameRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetOrderGameByOrderIdAndGameIdAsync_ValidIds_ReturnsOrderGame()
        {
            var orderGame = new OrderGame { OrderId = Guid.NewGuid(), GameId = Guid.NewGuid(), Quantity = 1 };

            using (var context = new DataContext(_options))
            {
                context.OrderGames.Add(orderGame);
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);
                var result = await repository.GetOrderGameByOrderIdAndGameIdAsync(orderGame.OrderId, orderGame.GameId);

                Assert.NotNull(result);
                Assert.Equal(orderGame.OrderId, result.OrderId);
                Assert.Equal(orderGame.GameId, result.GameId);
            }
        }

        [Fact]
        public async Task GetOrderGameByOrderIdAndGameIdAsync_InvalidIds_ReturnsNull()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);
                var result = await repository.GetOrderGameByOrderIdAndGameIdAsync(Guid.NewGuid(), Guid.NewGuid());

                Assert.Null(result);
            }
        }

        [Fact]
        public async Task AddOrderGameAsync_ValidOrderGame_AddsSuccessfully()
        {
            var validOrderGame = new OrderGame
            {
                OrderId = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Quantity = 2,
                Price = 100,
                Discount = 10,
            };

            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);

                await repository.AddOrderGameAsync(validOrderGame);

                var addedOrderGame = await context.OrderGames
                    .FirstOrDefaultAsync(og => og.OrderId == validOrderGame.OrderId && og.GameId == validOrderGame.GameId);

                Assert.NotNull(addedOrderGame);
                Assert.Equal(validOrderGame.OrderId, addedOrderGame.OrderId);
                Assert.Equal(validOrderGame.GameId, addedOrderGame.GameId);
                Assert.Equal(validOrderGame.Quantity, addedOrderGame.Quantity);
                Assert.Equal(validOrderGame.Price, addedOrderGame.Price);
                Assert.Equal(validOrderGame.Discount, addedOrderGame.Discount);
            }
        }

        [Fact]
        public async Task RemoveOrderGameAsync_ValidOrderGame_RemovesSuccessfully()
        {
            var orderGame = new OrderGame
            {
                OrderId = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Quantity = 2,
                Price = 100,
                Discount = 10,
            };

            using (var context = new DataContext(_options))
            {
                context.OrderGames.Add(orderGame);
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);

                await repository.RemoveOrderGameAsync(orderGame);
            }

            using (var context = new DataContext(_options))
            {
                var removedOrderGame = await context.OrderGames
                    .FirstOrDefaultAsync(og => og.OrderId == orderGame.OrderId && og.GameId == orderGame.GameId);

                Assert.Null(removedOrderGame);
            }
        }

        [Fact]
        public async Task UpdateOrderGameAsync_ValidOrderGame_UpdatesSuccessfully()
        {
            var orderGame = new OrderGame
            {
                OrderGameId = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Quantity = 2,
                Price = 100,
                Discount = 10,
            };

            using (var context = new DataContext(_options))
            {
                context.OrderGames.Add(orderGame);
                await context.SaveChangesAsync();
            }

            var updatedOrderGame = new OrderGame
            {
                OrderGameId = orderGame.OrderGameId,
                OrderId = orderGame.OrderId,
                GameId = orderGame.GameId,
                Quantity = 3,
                Price = 120,
                Discount = 15,
            };

            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);
                await repository.UpdateOrderGameAsync(updatedOrderGame);
            }

            using (var context = new DataContext(_options))
            {
                var result = await context.OrderGames.FindAsync(orderGame.OrderGameId);

                Assert.NotNull(result);
                Assert.Equal(3, result.Quantity);
                Assert.Equal(120, result.Price);
                Assert.Equal(15, result.Discount);
            }
        }

       



        [Fact]
        public async Task GetOrderGamesByOrderIdAsync_ValidOrderId_ReturnsOrderGames()
        {
            var orderId = Guid.NewGuid();
            var orderGames = new List<OrderGame>
        {
            new OrderGame { OrderId = orderId, GameId = Guid.NewGuid(), Quantity = 1 },
            new OrderGame { OrderId = orderId, GameId = Guid.NewGuid(), Quantity = 2 },
        };

            using (var context = new DataContext(_options))
            {
                context.OrderGames.AddRange(orderGames);
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);
                var result = await repository.GetOrderGamesByOrderIdAsync(orderId);

                Assert.NotNull(result);
                Assert.Equal(orderGames.Count, result.Count());
            }
        }

        [Fact]
        public async Task GetTotalQuantityByGameIdAsync_ValidGameId_ReturnsTotalQuantity()
        {
            var gameId = Guid.NewGuid();
            var orderGames = new List<OrderGame>
        {
            new OrderGame { OrderId = Guid.NewGuid(), GameId = gameId, Quantity = 3 },
            new OrderGame { OrderId = Guid.NewGuid(), GameId = gameId, Quantity = 2 }
        };

            using (var context = new DataContext(_options))
            {
                context.OrderGames.AddRange(orderGames);
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);
                var result = await repository.GetTotalQuantityByGameIdAsync(gameId);

                Assert.Equal(5, result);
            }
        }

        [Fact]
        public async Task GetAllOrderGamesAsync_ExistTwoOrders_ReturnsAllOrderGames()
        {
            var game1 = new Game
            {
                Id = Guid.NewGuid(),
                Name = "Game 1",
                Description = "Description of Game 1",
                Key = "game1-key",
                Price = 10.0,
            };

            var game2 = new Game
            {
                Id = Guid.NewGuid(),
                Name = "Game 2",
                Description = "Description of Game 2",
                Key = "game2-key",
                Price = 15.0,
            };

            var order1 = new Order
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Status = OrderStatus.Open,
            };

            var order2 = new Order
            {
                Id = Guid.NewGuid(),
                Date = DateTime.Now,
                Status = OrderStatus.Open,
            };

            var orderGames = new List<OrderGame>
        {
            new OrderGame { OrderId = order1.Id, GameId = game1.Id, Quantity = 1 },
            new OrderGame { OrderId = order2.Id, GameId = game2.Id, Quantity = 2 },
        };

            using (var context = new DataContext(_options))
            {
                context.Games.AddRange(game1, game2);
                context.Orders.AddRange(order1, order2);
                context.OrderGames.AddRange(orderGames);
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);
                var result = await repository.GetAllOrderGamesAsync();

                Assert.NotNull(result);
                Assert.Equal(orderGames.Count, result.Count());
            }
        }

        [Fact]
        public async Task GetAllOrderGamesAsync_EmptyDatabase_ReturnsEmptyCollection()
        {
            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);

                var result = await repository.GetAllOrderGamesAsync();

                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task GetOrderGamesByOrderIdAsync_InvalidOrderId_ReturnsEmptyList()
        {
            var orderId = Guid.NewGuid();

            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);

                var result = await repository.GetOrderGamesByOrderIdAsync(orderId);

                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task GetTotalQuantityByGameIdAsync_InvalidGameId_ReturnsZero()
        {
            var gameId = Guid.NewGuid();

            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);

                var result = await repository.GetTotalQuantityByGameIdAsync(gameId);

                Assert.Equal(0, result);
            }
        }

        [Fact]
        public async Task RemoveOrderGameByIdAsync_ExistingOrderGame_RemovesOrderGame()
        {
            var orderGameId = Guid.NewGuid();

            var order = new Order
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                Status = OrderStatus.Open
            };

            var game = new Game
            {
                Id = Guid.NewGuid(),
                Name = "Test Game",
                Key = "test-game-key",
                Description = "Test game description",
                Price = 59.99,
                UnitInStock = 20,
                Discount = 10,
                PublisherId = Guid.NewGuid(),
                PublishedDate = DateTime.UtcNow,
                ViewCount = 100
            };

            var orderGame = new OrderGame
            {
                OrderGameId = orderGameId,
                OrderId = order.Id,
                Order = order,
                GameId = game.Id,
                Game = game,
                Quantity = 2,
                Price = 59.99,
                Discount = 10
            };

            using (var context = new DataContext(_options))
            {
                context.Orders.Add(order);
                context.Games.Add(game);
                context.OrderGames.Add(orderGame);
                await context.SaveChangesAsync();
            }

            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);

                await repository.RemoveOrderGameByIdAsync(orderGameId);

                var deletedOrderGame = await context.OrderGames.FindAsync(orderGameId);
                Assert.Null(deletedOrderGame);
            }
        }

        [Fact]
        public async Task RemoveOrderGameByIdAsync_NonExistingOrderGame_NoActionTaken()
        {
            var nonExistingId = Guid.NewGuid();

            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);

                await repository.RemoveOrderGameByIdAsync(nonExistingId);
            }

            using (var context = new DataContext(_options))
            {
                var allOrderGames = await context.OrderGames.ToListAsync();
                Assert.Empty(allOrderGames);
            }
        }

        [Fact]
        public async Task GetOrderGameByOrderIdAndGameIdAsync_ExistingOrderAndGame_ReturnsOrderGame()
        {
            var orderId = Guid.NewGuid();
            var gameId = Guid.NewGuid();
            var orderGameId = Guid.NewGuid();

            var order = new Order
            {
                Id = orderId,
                Date = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                Status = OrderStatus.Open,
            };

            var game = new Game
            {
                Id = gameId,
                Name = "Test Game",
                Key = "test-game-key",
                Description = "Test game description",
                Price = 59.99,
                UnitInStock = 20,
                Discount = 10,
                PublisherId = Guid.NewGuid(),
                PublishedDate = DateTime.UtcNow,
                ViewCount = 100,
            };

            var orderGame = new OrderGame
            {
                OrderGameId = orderGameId,
                OrderId = orderId,
                Order = order,
                GameId = gameId,
                Game = game,
                Quantity = 1,
                Price = 59.99,
                Discount = 10,
            };

            using (var context = new DataContext(_options))
            {
                context.Orders.Add(order);
                context.Games.Add(game);
                context.OrderGames.Add(orderGame);
                await context.SaveChangesAsync();
            }

            OrderGame result;

            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);
                result = await repository.GetOrderGameByOrderIdAndGameIdAsync(orderId, gameId);
            }

            Assert.NotNull(result);
            Assert.Equal(orderId, result.OrderId);
            Assert.Equal(gameId, result.GameId);
            Assert.Equal(orderGameId, result.OrderGameId);
            Assert.Equal(59.99, result.Price);
            Assert.Equal(1, result.Quantity);
            Assert.Equal(10, result.Discount);
        }

        [Fact]
        public async Task GetOrderGameByIdAsync_NonExistingOrderGame_ReturnsNull()
        {
            var nonExistingOrderGameId = Guid.NewGuid();

            OrderGame result;
            using (var context = new DataContext(_options))
            {
                var repository = new OrderGameRepository(context);
                result = await repository.GetOrderGameByIdAsync(nonExistingOrderGameId);
            }

            Assert.Null(result);
        }
    }
}
