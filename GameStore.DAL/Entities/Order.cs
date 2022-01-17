using System;
using System.Collections.Generic;

namespace GameStore.DAL.Entities
{
    public class Order : SoftDeletable
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }

        public DateTime DateTime { get; set; }

        public int PaymentStatusCode { get; set; }

        public decimal TotalPrice { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
    }
}
