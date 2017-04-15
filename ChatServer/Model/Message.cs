using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChatServer.Model
{
    class Message
    {
        [Key]
        public int messageId { get; set; }
        public string messageText { get; set; }
        public int senderId { get; set; }
        public int targetId { get; set; }

    }

}
