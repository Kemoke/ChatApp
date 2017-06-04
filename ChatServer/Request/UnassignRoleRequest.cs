namespace ChatServer.Request
{
    public class UnassignRoleRequest
    {
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}