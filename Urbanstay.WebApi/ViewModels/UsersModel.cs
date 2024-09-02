using System;

namespace Urbanstay.WebApi.ViewModels
{
    public class UsersModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string ProfilePic { get; set; }
        public string SysAdmin { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public string AddedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
