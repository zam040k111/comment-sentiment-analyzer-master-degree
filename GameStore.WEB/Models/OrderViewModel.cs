using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Interfaces;

namespace GameStore.WEB.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }

        public DateTime DateTime { get; set; }

        public int PaymentStatusCode { get; set; }

        public decimal TotalPrice => OrderDetails.Sum(o => o.Price);

        public List<OrderDetailViewModel> OrderDetails { get; set; }

        public IPayment PaymentMethod { get; set; }
    }
}
