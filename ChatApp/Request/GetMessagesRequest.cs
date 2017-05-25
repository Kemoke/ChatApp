namespace ChatApp.Request
{
    public class GetMessagesRequest : BaseRequest
    {
        public int ChannelId { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
    }
}
