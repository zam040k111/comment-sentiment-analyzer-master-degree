using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface IOrderDetailService : IValidatedService<OrderDetailDto>
    {
        OrderDetailDto GetByGameKey(string key);
    }
}
