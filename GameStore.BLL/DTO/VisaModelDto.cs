
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class VisaModelDto : IPaymentInfo
    {
        public string HolderName { get; set; }

        public string CardNumber { get; set; }
        
        public int Month { get; set; }
        
        public int Year { get; set; }
        
        public int? Cvc { get; set; }

        public OrderDto Order { get; set; }
    }
}
