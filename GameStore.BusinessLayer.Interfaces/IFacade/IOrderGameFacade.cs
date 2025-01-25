// <copyright file="IOrderGameFacade.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.IFacade
{
    using GameStore.BusinessLayer.Interfaces.ResponseDto;

    public interface IOrderGameFacade
    {
        Task<IEnumerable<GetOrderGameResponse>> GetOrderDetailsByOrderIdAsync(string orderId);

        Task<IEnumerable<GetOrderGameResponse>> GetAllOrderDetailsAsync();

        Task UpdateOrderDetailQuantityAsync(string orderDetailId, int newQuantity);

        Task RemoveOrderGameAndDetailsByOrderIdAsync(string orderId);
    }
}
