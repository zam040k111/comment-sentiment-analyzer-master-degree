using System;
using System.Collections.Generic;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.Services.Payment.Models
{
    public static class Payments
    {
        public static readonly Dictionary<string, PaymentStatus> Code = new Dictionary<string, PaymentStatus>
        {
            {PaymentTypes.Bank, PaymentStatus.Pending},
            {PaymentTypes.BoxTerminal, PaymentStatus.Pending},
            {PaymentTypes.Visa, PaymentStatus.Success}
        };

        public static readonly Dictionary<string, Type> TypeByName = new Dictionary<string, Type>
        {
            {PaymentTypes.Bank, typeof(Bank)},
            {PaymentTypes.BoxTerminal, typeof(BoxTerminal)},
            {PaymentTypes.Visa, typeof(Visa)}
        };

        public static IPayment GetInstanceByName(string name, IOrderService orderService) =>
            (IPayment)Activator.CreateInstance(TypeByName[name], orderService);
    }
}
