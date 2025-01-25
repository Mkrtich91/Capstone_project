using GameStore.BusinessLayer.Interfaces.ResponseDto;
namespace MongoDB.Services.IServices
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<GetOrderGameResponse>> GetAllOrderDetailsAsync();
        Task<IEnumerable<GetOrderGameResponse>> GetOrderDetailsByOrderIdAsync(string orderId);
    }

}
