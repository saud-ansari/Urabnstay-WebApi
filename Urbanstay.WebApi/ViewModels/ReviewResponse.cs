using System;

namespace Urbanstay.WebApi.ViewModels
{
    public class ReviewResponse
    {
        public int ReviewId { get; set; }
        public int PropertyId { get; set; }
        public int ReviewerId { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
