namespace ChatApp.Request
{
    public class AssignRoleRequest
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public int TeamId { get; set; }
    }
}
