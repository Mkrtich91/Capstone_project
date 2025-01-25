namespace GameStore.DataAccessLayer.Interfaces.Entities
{
    using System;
    using System.Collections.Generic;

    public class Order
    {
        public Guid Id { get; set; }

        public DateTime? Date { get; set; }

        public Guid CustomerId { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<OrderGame> OrderGames { get; set; }

    }

    public enum OrderStatus
    {
        Open = 0,
        Checkout = 1,
        Paid = 2,
        Cancelled = 3,
        Shipped = 4,
    }
}
