using System;

namespace ChatApp.Model
{
    public class UserInfo
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PictureUrl { get; set; }
        public string Gender { get; set; }
    }
}
