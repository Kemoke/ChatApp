using ChatServer.Model;

namespace ChatServer.Response
{
    public class LoginResponse
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}