using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Payment.Models;
using GameStore.BLL.Services.Validation;

namespace GameStore.BLL.Services.Payment
{
    public class Bank : IPayment
    {
        private readonly IOrderService _orderService;

        public Bank(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Result<OrderDto> Pay(OrderDto order)
        {
            order.PaymentStatusCode = (int) Payments.Code[PaymentTypes.Bank];

            return _orderService.Buy(order);
        }
    }
}
