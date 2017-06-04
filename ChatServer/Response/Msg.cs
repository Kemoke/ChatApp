namespace ChatServer.Response
{
    public class Msg
    {
        public string Message { get; set; }

        public Msg(string message)
        {
            Message = message;
        }
    }
}