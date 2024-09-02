using System;
using System.Collections.Generic;

#nullable disable

namespace Urbanstay.WebApi.Models
{
    public partial class Booking
    {
        public Booking()
        {
            Payments = new HashSet<Payment>();
        }

        public int BookingId { get; set; }
        public int PropertyId { get; set; }
        public int GuestId { get; set; }
        public int HostId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfGuests { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual User Guest { get; set; }
        public virtual User Host { get; set; }
        public virtual Property Property { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
