using System;

namespace Urbanstay.WebApi.ViewModels
{
    public class AddOrder
    {
        public int PropertyId { get; set; }
        public int GuestId { get; set; }
        public int HostId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalPrice { get; set; }
        //public string Status { get; set; }
        //public DateTime? CreatedAt { get; set; }
    }

    public class EditOrder
    {
        public int PropertyId { get; set; }
        public int GuestId { get; set; }
        public int HostId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
