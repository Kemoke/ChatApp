namespace ChatApp.Model
{
    public class Message : Entity
    {
        public string MessageText { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public int ChannelId { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Target { get; set; }
        public virtual Channel Channel { get; set; }
    }

}
