using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.DTO;
using GameStore.WEB.Extensions;
using GameStore.WEB.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GameStore.WEB.Services
{
    public class CartService : ICartService
    {
        public ISession Add(GameDto game, ISession session)
        {
            game.GameGenres = null;
            game.GamePlatformTypes = null;
            var orderDetail = new OrderDetailDto
            {
                ProductId = game.Id,
                ProductKey = game.Key,
                Price = game.Price,
                Quantity = 1,
                Product = game
            };
            var cartOrder = session.Get<OrderDto>("cart");

            if (cartOrder == null)
            {
                var order = new OrderDto {OrderDetails = new List<OrderDetailDto> {orderDetail}};
                session.Set("cart", order);
                session.SetInt32("countItems", order.OrderDetails.Count);
            }
            else
            {
                var addedGame = cartOrder.OrderDetails.FirstOrDefault(od => od.ProductKey == game.Key);

                if (addedGame != null)
                {
                    addedGame.Quantity++;
                }
                else
                {
                    cartOrder.OrderDetails.Add(orderDetail);
                }

                session.Set("cart", cartOrder);
                session.SetInt32("countItems", cartOrder.OrderDetails.Count);
            }

            return session;
        }

        public ISession Remove(string gameKey, ISession session)
        {
            var cartOrder = session.Get<OrderDto>("cart");
            cartOrder.OrderDetails.Remove(cartOrder.OrderDetails.Find(i => i.ProductKey == gameKey));
            session.Set("cart", cartOrder);
            session.SetInt32("countItems", cartOrder.OrderDetails.Count);

            return session;
        }

        public ISession RemoveAll(ISession session)
        {
            session.Clear();

            return session;
        }

        public OrderDto GetAll(ISession session) => session.Get<OrderDto>("cart");

        public OrderDto UpdateQuantity(ISession session, int itemIndex, short newQuantity)
        {
            var cartItems = session.Get<OrderDto>("cart");
            cartItems.OrderDetails[itemIndex].Quantity = newQuantity;
            session.Set("cart", cartItems);

            return cartItems;
        }
    }
}
