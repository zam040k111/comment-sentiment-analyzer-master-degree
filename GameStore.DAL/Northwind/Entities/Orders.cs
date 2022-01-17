using System;
using System.Collections.Generic;

namespace GameStore.DAL.Northwind.Entities
{
    public class Orders : MongoModel
    {
        public int OrderId { get; set; }

        public string CustomerId { get; set; }

        public DateTime DateTime { get; set; }

        public ICollection<DAL.Entities.OrderDetail> OrderDetails { get; set; }
    }
}