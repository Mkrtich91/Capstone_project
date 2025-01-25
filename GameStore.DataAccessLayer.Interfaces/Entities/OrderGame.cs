using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DataAccessLayer.Interfaces.Entities
{
    public class OrderGame
    {
        public Guid OrderGameId { get; set; }

        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        public Guid GameId { get; set; }

        public Game Game { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }

        public int? Discount { get; set; }
    }
}
