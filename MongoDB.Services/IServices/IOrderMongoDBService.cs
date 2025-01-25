using GameStore.BusinessLayer.Interfaces.ResponseDto;

namespace MongoDB.Services.IServices
{
    public interface IOrderMongoDBService
    {
        Task<GetOrderResponse> GetOrderByIdAsync(string orderId);

        Task<IEnumerable<GetOrderResponse>> GetAllOrdersAsync();
        

    }
}
