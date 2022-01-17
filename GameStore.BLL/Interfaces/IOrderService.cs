using GameStore.BLL.DTO;
using GameStore.BLL.Services.Validation;

namespace GameStore.BLL.Interfaces
{
    public interface IOrderService : IValidatedService<OrderDto>
    {
        Result<OrderDto> Buy(OrderDto order);

        void AddPaymentInfo(IPaymentInfo info);
    }
}
