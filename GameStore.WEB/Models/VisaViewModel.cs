using System.ComponentModel.DataAnnotations;

namespace GameStore.WEB.Models
{
    public class VisaViewModel
    {
        public OrderViewModel Order { get; set; }

        [Required]
        [RegularExpression(@"^[^\s]+( [^\s]+)+$", ErrorMessage = "The Holder name look like this: 'Ivan Ivanov'")]
        public string HolderName { get; set; }

        [Required]
        [RegularExpression(@"\d{4} \d{4} \d{4} \d{4}", ErrorMessage = "The card number look like this: '1111 2222 3333 4444'")]
        [MaxLength(19)]
        public string CardNumber { get; set; }

        [Required]
        public int Month { get; set; }
        
        [Required]
        public int Year { get; set; }
        
        [Required]
        [RegularExpression(@"\d{3}", ErrorMessage = "The CVC/CVV look like this: '123'")]
        public int? Cvc { get; set; }
    }
}
