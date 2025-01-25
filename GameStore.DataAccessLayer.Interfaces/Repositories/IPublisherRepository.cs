namespace GameStore.DataAccessLayer.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using GameStore.DataAccessLayer.Interfaces.Entities;

    public interface IPublisherRepository
    {
        Task<Publisher> AddPublisherAsync(Publisher publisher);

        Task<Publisher> GetPublisherByIdAsync(Guid id);

        Task<Publisher> GetPublisherByNameAsync(string name);

        Task<IEnumerable<Publisher>> GetAllPublishersAsync();

        Task<Publisher> UpdatePublisherAsync(Publisher publisher);

        Task DeletePublisherAsync(Guid id);

        Task<Publisher> GetPublisherByGameKeyAsync(string key);

    }
}
