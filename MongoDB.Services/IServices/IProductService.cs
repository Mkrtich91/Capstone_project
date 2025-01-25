using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MongoDB.Services.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<GetGameResponse>> GetAllProductsAsync();
        Task<GetGameResponse> FindProductByIdAsync(string productId);
        Task<IEnumerable<GameOverviewDto>> GetAllProductOverviewsAsync();

    }

}
