namespace ChatApp.Model
{
    public class Message : Entity
    {
        public string MessageText { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public int ChannelId { get; set; }

        public User Sender { get; set; }
        public User Target { get; set; }
        public Channel Channel { get; set; }
    }

}
