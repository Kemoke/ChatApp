using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChatServer.Model
{
    public class Token
    {
        public int TokenId { get; set; }
        public string  TokenHash { get; set; }
        public int UserId { get; set; }
        
        [Required]
        public virtual User User{ get; set; }

    }
}
