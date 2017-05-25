using System.ComponentModel.DataAnnotations;

namespace ChatApp.Model
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
