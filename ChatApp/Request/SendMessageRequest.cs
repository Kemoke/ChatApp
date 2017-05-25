namespace ChatApp.Request
{
    public class SendMessageRequest : BaseRequest
    {
        public string MessageText { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public int ChannelId { get; set; }
    }
}
