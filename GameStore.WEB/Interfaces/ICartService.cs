using GameStore.BLL.DTO;
using Microsoft.AspNetCore.Http;

namespace GameStore.WEB.Interfaces
{
    public interface ICartService
    {
        ISession Add(GameDto game, ISession session);
        ISession Remove(string gameKey, ISession session);
        ISession RemoveAll(ISession session);
        OrderDto GetAll(ISession session);
        OrderDto UpdateQuantity(ISession session, int itemIndex, short newQuantity);
    }
}
