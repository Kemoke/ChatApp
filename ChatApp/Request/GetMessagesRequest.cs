namespace ChatApp.Request
{
    public class GetMessagesRequest
    {
        public int ChannelId { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
    }
}
