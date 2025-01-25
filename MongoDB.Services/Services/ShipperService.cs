using GameStore.BusinessLayer.Interfaces.Exceptions;
using MongoDB.Entities.NoSQLDTOs;
using MongoDB.Repositories.IRepository;
using MongoDB.Services.IServices;

namespace MongoDB.Services.Services
{
    public class ShipperService : IShipperService
    {
        private readonly IShipperRepository _shipperRepository;

        public ShipperService(IShipperRepository shipperRepository)
        {
            _shipperRepository = shipperRepository;
        }

        public async Task<IEnumerable<GetShipperResponse>> GetAllShippersAsync()
        {
            var shippers = await _shipperRepository.GetAllShippersAsync();
            return shippers.Select(shipper => new GetShipperResponse
            {
                ShipperId = shipper.ShipperID,
                CompanyName = shipper.CompanyName,
                Phone = shipper.Phone
            }).ToList();
        }

        public async Task<GetShipperResponse> GetShipperByIdAsync(int shipperId)
        {
            var shipper = await _shipperRepository.GetShipperByIdAsync(shipperId);
            if (shipper == null)
            {
                throw new NotFoundException($"Shipper with ID {shipperId} not found.");
            }

            return new GetShipperResponse
            {
                ShipperId = shipper.ShipperID,
                CompanyName = shipper.CompanyName,
                Phone = shipper.Phone
            };
        }
    }
}
