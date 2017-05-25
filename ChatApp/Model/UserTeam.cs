namespace ChatApp.Model
{
    public class UserTeam : Entity
    {
        public int UserId { get; set; }
        public int TeamId { get; set; }
        public int RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Team Team { get; set; }
        public virtual Role Role { get; set; }
    }
}
