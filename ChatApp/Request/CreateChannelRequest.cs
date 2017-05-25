namespace ChatApp.Request
{
    public class CreateChannelRequest : BaseRequest
    {
        public string ChannelName { get; set; }
        public int TeamId { get; set; }
    }
}
