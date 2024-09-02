using System;
using System.Collections.Generic;

#nullable disable

namespace Urbanstay.WebApi.Models
{
    public partial class User
    {
        public User()
        {
            BookingGuests = new HashSet<Booking>();
            BookingHosts = new HashSet<Booking>();
            InverseAddedBy = new HashSet<User>();
            InverseModifiedBy = new HashSet<User>();
            Properties = new HashSet<Property>();
            Reviews = new HashSet<Review>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
        public string ProfilePic { get; set; }
        public bool? SysAdmin { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public int? AddedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedById { get; set; }

        public virtual User AddedBy { get; set; }
        public virtual User ModifiedBy { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Booking> BookingGuests { get; set; }
        public virtual ICollection<Booking> BookingHosts { get; set; }
        public virtual ICollection<User> InverseAddedBy { get; set; }
        public virtual ICollection<User> InverseModifiedBy { get; set; }
        public virtual ICollection<Property> Properties { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
