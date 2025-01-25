namespace MongoDB.Services.IServices
{
    using GameStore.BusinessLayer.Interfaces.ResponseDto;
    public interface ICategoryService
    {
        Task<List<GetGenreResponse>> GetAllGenresAsync();

        Task<GetGenreResponse> GetGenreByIdAsync(string categoryId);
    }

}
