using System;

namespace Urbanstay.WebApi.ViewModels
{
    public class PaymentModel
    {
        //public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }
    }
}
