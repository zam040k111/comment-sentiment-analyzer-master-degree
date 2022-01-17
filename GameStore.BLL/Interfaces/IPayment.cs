using GameStore.BLL.DTO;
using GameStore.BLL.Services.Validation;

namespace GameStore.BLL.Interfaces
{
    public interface IPayment
    {
        Result<OrderDto> Pay(OrderDto order);
    }
}
