﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Urbanstay.WebApi.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
