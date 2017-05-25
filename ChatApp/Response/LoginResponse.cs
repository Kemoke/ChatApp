using ChatApp.Model;

namespace ChatApp.Response
{
    public class LoginResponse
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}