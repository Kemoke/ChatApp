namespace ChatServer.WebSockets
{
    public class NotificationMessage
    {
        public string Token { get; set; }
        public int OldId { get; set; }
        public int NewId { get; set; }
    }
}