namespace GameStore.BusinessLayer.Services
{
    using GameStore.BusinessLayer.Interfaces.DTO;
    using GameStore.BusinessLayer.Interfaces.Exceptions;
    using GameStore.BusinessLayer.Interfaces.RequestDto;
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    using GameStore.BusinessLayer.Interfaces.Services;
    using GameStore.DataAccessLayer.Interfaces.Entities;
    using GameStore.DataAccessLayer.Interfaces.Repositories;

    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;
        private readonly IGameRepository _gameRepository;

        public PublisherService(IPublisherRepository publisherRepository, IGameRepository gameRepository)
        {
            _publisherRepository = publisherRepository;
            _gameRepository = gameRepository;
        }

        public async Task<Publisher> AddPublisherAsync(PublisherRequest publisherDto)
        {
            if (string.IsNullOrEmpty(publisherDto.Publisher.CompanyName))
            {
                throw new ArgumentException("CompanyName cannot be null or empty.");
            }

            var publisher = new Publisher
            {
                Id = Guid.NewGuid(),
                CompanyName = publisherDto.Publisher.CompanyName,
                HomePage = publisherDto.Publisher.HomePage,
                Description = publisherDto.Publisher.Description,
            };

            return await _publisherRepository.AddPublisherAsync(publisher);
        }

        public async Task<GetPublisherResponse> GetPublisherByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out var guidId))
            {
                throw new NotFoundException($"Invalid ID format: {id}");
            }

            var publisher = await _publisherRepository.GetPublisherByIdAsync(guidId);

            if (publisher == null)
            {
                throw new NotFoundException($"Publisher with ID {id} not found.");
            }

            return new GetPublisherResponse
            {
                Id = publisher.Id,
                CompanyName = publisher.CompanyName,
                HomePage = publisher.HomePage,
                Description = publisher.Description,
            };
        }

        public async Task<GetPublisherResponse> GetPublisherByNameAsync(string companyName)
        {
            if (string.IsNullOrEmpty(companyName))
            {
                throw new ArgumentException("Company name cannot be null or empty.", nameof(companyName));
            }

            var publisher = await _publisherRepository.GetPublisherByNameAsync(companyName);

            if (publisher == null)
            {
                throw new NotFoundException($"Publisher with name '{companyName}' not found.");
            }

            var response = new GetPublisherResponse
            {
                Id = publisher.Id,
                CompanyName = publisher.CompanyName,
                HomePage = publisher.HomePage,
                Description = publisher.Description,
            };

            return response;
        }

        public async Task<IEnumerable<GetPublisherResponse>> GetAllPublishersAsync()
        {
            var publishers = await _publisherRepository.GetAllPublishersAsync();

            return publishers.Select(publisher => new GetPublisherResponse
            {
                Id = publisher.Id,
                CompanyName = publisher.CompanyName,
                HomePage = publisher.HomePage,
                Description = publisher.Description,
            });
        }

        public async Task<PublisherUpdateDto> UpdatePublisherAsync(UpdatePublisherRequest request)
        {
            if (request == null || request.Publisher == null)
            {
                throw new ArgumentNullException(nameof(request), "Invalid or empty update request.");
            }

            var publisherDto = request.Publisher;

            if (publisherDto.Id == Guid.Empty ||
                string.IsNullOrWhiteSpace(publisherDto.CompanyName) ||
                string.IsNullOrWhiteSpace(publisherDto.HomePage) ||
                string.IsNullOrWhiteSpace(publisherDto.Description))
            {
                throw new ArgumentException("All fields are mandatory and should be populated with correct data.");
            }

            var publisher = await _publisherRepository.GetPublisherByIdAsync(publisherDto.Id);
            if (publisher == null)
            {
                throw new Exception("Publisher not found.");
            }

            publisher.CompanyName = publisherDto.CompanyName;
            publisher.HomePage = publisherDto.HomePage;
            publisher.Description = publisherDto.Description;

            await _publisherRepository.UpdatePublisherAsync(publisher);

            return new PublisherUpdateDto
            {
                Id = publisher.Id,
                CompanyName = publisher.CompanyName,
                HomePage = publisher.HomePage,
                Description = publisher.Description,
            };
        }

        public async Task DeletePublisherAsync(Guid id)
        {
            var existingPublisher = await _publisherRepository.GetPublisherByIdAsync(id);
            if (existingPublisher == null)
            {
                throw new NotFoundException($"Publisher with ID {id} not found.");
            }

            await _publisherRepository.DeletePublisherAsync(id);
        }

        public async Task<GetPublisherResponse> GetPublisherByGameKeyAsync(string key)
        {
            var publisher = await _publisherRepository.GetPublisherByGameKeyAsync(key);

            if (publisher == null)
            {
                throw new NotFoundException($"Publisher not found for game key '{key}'.");
            }

            var response = new GetPublisherResponse
            {
                Id = publisher.Id,
                CompanyName = publisher.CompanyName,
                Description = publisher.Description,
                HomePage = publisher.HomePage,
            };

            return response;
        }

        public async Task<List<PublisherGameDto>> GetGamesByPublisherNameAsync(string companyName)
        {
            var publisher = await _publisherRepository.GetPublisherByNameAsync(companyName);
            if (publisher == null)
            {
                throw new NotFoundException($"Publisher with name '{companyName}' not found.");
            }

            var games = await _gameRepository.GetGamesByPublisherIdAsync(publisher.Id);
            if (games == null || games.Count == 0)
            {
                throw new NotFoundException($"No games found for publisher '{companyName}'.");
            }

            return games.Select(game => new PublisherGameDto
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                Key = game.Key,
                Price = game.Price,
                Discount = game.Discount,
                UnitInStock = game.UnitInStock,
            }).ToList();
        }
    }
}
