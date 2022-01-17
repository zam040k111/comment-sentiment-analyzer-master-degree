using GameStore.BLL.DTO;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Payment.Models;
using GameStore.BLL.Services.Validation;

namespace GameStore.BLL.Services.Payment
{
    public class Visa : IPayment
    {
        private readonly IOrderService _orderService;

        public Visa(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Result<OrderDto> Pay(OrderDto order)
        {
            order.PaymentStatusCode = (int) Payments.Code[PaymentTypes.Visa];

            return _orderService.Buy(order);
        }

        public void AddPaymentInfo(VisaModelDto visaModel)
        {
            _orderService.AddPaymentInfo(visaModel);
        }
    }
}
