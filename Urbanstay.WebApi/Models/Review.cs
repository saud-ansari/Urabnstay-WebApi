using System;
using System.Collections.Generic;

#nullable disable

namespace Urbanstay.WebApi.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public int PropertyId { get; set; }
        public int ReviewerId { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Property Property { get; set; }
        public virtual User Reviewer { get; set; }
    }
}
