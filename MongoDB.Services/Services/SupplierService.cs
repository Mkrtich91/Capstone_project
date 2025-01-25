using AutoMapper;
using GameStore.BusinessLayer.Interfaces.Exceptions;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using MongoDB.Repositories.IRepository;
using MongoDB.Services.IServices;

namespace MongoDB.Services.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierService(ISupplierRepository supplierRepository, IMapper mapper)
        {
            _supplierRepository = supplierRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetPublisherResponse>> GetAllPublishersAsync()
        {
            var suppliers = await _supplierRepository.GetAllSuppliersAsync();
            return _mapper.Map<IEnumerable<GetPublisherResponse>>(suppliers);
        }

        public async Task<GetPublisherResponse> GetPublisherByIdAsync(string supplierId)
        {
            if (!int.TryParse(supplierId, out var parsedSupplierId))
            {
                throw new ArgumentException($"Invalid ID format: {supplierId}");
            }

            var supplier = await _supplierRepository.GetSupplierByIdAsync(parsedSupplierId);

            if (supplier == null)
            {
                throw new NotFoundException($"Supplier with ID {supplierId} not found.");
            }

            return _mapper.Map<GetPublisherResponse>(supplier);
        }

        public async Task<GetPublisherResponse> GetPublisherByNameAsync(string supplierName)
        {
            var supplier = await _supplierRepository.GetSupplierByNameAsync(supplierName);

            if (supplier == null)
            {
                throw new NotFoundException($"Supplier with name '{supplierName}' not found.");
            }

            return _mapper.Map<GetPublisherResponse>(supplier);
        }
    }
}
