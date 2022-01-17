
using System;
using System.Collections.Generic;

namespace GameStore.BLL.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }

        public DateTime DateTime { get; set; }

        public int PaymentStatusCode { get; set; }

        public decimal TotalPrice { get; set; }

        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}
