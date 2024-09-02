using System;
using System.Collections.Generic;

#nullable disable

namespace Urbanstay.WebApi.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public int? AddedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedById { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
