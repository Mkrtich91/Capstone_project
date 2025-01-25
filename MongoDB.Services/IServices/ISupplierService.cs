using GameStore.BusinessLayer.Interfaces.DTO;
using GameStore.BusinessLayer.Interfaces.ResponseDto;
using MongoDB.Bson;
using MongoDB.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Services.IServices
{
    public interface ISupplierService
    {
        Task<IEnumerable<GetPublisherResponse>> GetAllPublishersAsync();
        Task<GetPublisherResponse> GetPublisherByIdAsync(string supplierId);
        Task<GetPublisherResponse> GetPublisherByNameAsync(string supplierName);
    }

}
