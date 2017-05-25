namespace ChatApp.Request
{
    public class CheckNewMessagesRequest : BaseRequest
    {
        public int MessageId { get; set; }
        public int ChannelId { get; set; }
    }
}
