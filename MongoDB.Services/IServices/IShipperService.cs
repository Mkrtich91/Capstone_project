using MongoDB.Entities.Converter;
using MongoDB.Entities.NoSQLDTOs;

namespace MongoDB.Services.IServices
{
    public interface IShipperService
    {
        Task<IEnumerable<GetShipperResponse>> GetAllShippersAsync();
        Task<GetShipperResponse> GetShipperByIdAsync(int shipperId);
    }
}
