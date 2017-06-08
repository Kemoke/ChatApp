namespace ChatApp.Request
{
    public class UnsignRoleRequest
    {
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}