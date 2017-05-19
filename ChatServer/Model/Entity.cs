using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatServer.Model
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
