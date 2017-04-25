using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatServer.Model
{
    public abstract class Entity
    {
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Updated { get; set; }
    }
}
