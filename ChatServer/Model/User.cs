using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChatServer.Model
{
    class User
    {
        [Key]
        public int userId { get; set; }
        public string  username { get; set; }
        public string fristName { get; set; }
        public string password { get; set; }
        public string copany { get; set; }
        public string country { get; set; }
        public DateTime  dateOfBirth { get; set; }
        public string pictureURL { get; set; }
        public string  gender { get; set; }
    }
}
