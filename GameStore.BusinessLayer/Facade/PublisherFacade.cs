using GameStore.BusinessLayer.Interfaces.Exceptions;
using GameStore.BusinessLayer.Interfaces.BaseClass;
using GameStore.BusinessLayer.Interfaces.IFacade;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using GameStore.BusinessLayer.Interfaces.Services;
using MongoDB.Services.IServices;

namespace GameStore.BusinessLayer.Facade
{
    public class PublisherFacade : FacadeBase, IPublisherFacade
    {
        private readonly ISupplierService _supplierService;
        private readonly IPublisherService _publisherService;

        public PublisherFacade(ISupplierService supplierService, IPublisherService publisherService)
        {
            _supplierService = supplierService;
            _publisherService = publisherService;
        }

        public async Task<IEnumerable<GetPublisherResponse>> GetAllPublishersAsync()
        {
            var suppliers = await _supplierService.GetAllPublishersAsync();

            var publishers = await _publisherService.GetAllPublishersAsync();

            return suppliers.Concat(publishers).ToList();
        }

        public async Task<GetPublisherResponse> GetPublisherByIdAsync(string id)
        {
         return
         await GetResponseOrNull(() => _publisherService.GetPublisherByIdAsync(id)) ??
         await GetResponseOrNull(() => _supplierService.GetPublisherByIdAsync(id)) ??
         throw new NotFoundException($"Publisher or Supplier with ID {id} not found.");
        }

        public async Task<GetPublisherResponse> GetPublisherByNameAsync(string companyName)
        {
            try
            {
                var suppliers = await _supplierService.GetAllPublishersAsync();
                var matchingSupplier = suppliers.FirstOrDefault(supplier => supplier.CompanyName.Contains(companyName, StringComparison.OrdinalIgnoreCase));
                if (matchingSupplier != null)
                {
                    return matchingSupplier;
                }

                var publishers = await _publisherService.GetAllPublishersAsync();
                var matchingPublisher = publishers.FirstOrDefault(publisher => publisher.CompanyName.Contains(companyName, StringComparison.OrdinalIgnoreCase));
                if (matchingPublisher != null)
                {
                    return matchingPublisher;
                }
            }
            catch (NotFoundException)
            {
                throw new NotFoundException($"No publisher or supplier found with name containing '{companyName}'.");
            }

            throw new ArgumentException($"Invalid argument");
        }
    }
}
