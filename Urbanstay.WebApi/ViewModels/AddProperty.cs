using System;
using Urbanstay.WebApi.Models;

namespace Urbanstay.WebApi.ViewModels
{
    public class AddProperty
    {
        public int PropertyId { get; set; }
        public int HostId { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string PropertyType { get; set; }
        public decimal PricePerNight { get; set; }
        public string AvailabilityCalendar { get; set; }
        public string HouseRules { get; set; }
        public bool? InstantBooking { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        public class EditProperty
        {
            public int PropertyId { get; set; }
            public int HostId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public bool IsActive { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public string ZipCode { get; set; }
            public string PropertyType { get; set; }
            public decimal PricePerNight { get; set; }
            public string AvailabilityCalendar { get; set; }
            public string HouseRules { get; set; }
            public bool? InstantBooking { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }

        public static implicit operator AddProperty(Property v)
        {
            throw new NotImplementedException();
        }
    }
}
