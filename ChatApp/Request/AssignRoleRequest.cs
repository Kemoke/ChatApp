namespace ChatApp.Request
{
    public class AssignRoleRequest : BaseRequest
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public int TeamId { get; set; }
    }
}
