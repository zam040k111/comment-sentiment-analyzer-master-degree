using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Payment.Models;
using GameStore.BLL.Services.Validation;

namespace GameStore.BLL.Services.Payment
{
    public class BoxTerminal : IPayment
    {
        private readonly IOrderService _orderService;

        public BoxTerminal(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Result<OrderDto> Pay(OrderDto order)
        {
            order.PaymentStatusCode = (int) Payments.Code[PaymentTypes.BoxTerminal];

            return _orderService.Buy(order);
        }
    }
}
