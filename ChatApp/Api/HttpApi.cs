using System.Threading.Tasks;
using ChatApp.Model;
using ChatApp.Request;
using ChatApp.Response;
using Refit;

namespace ChatApp.Api
{
    public static class ApiExtensions
    {
        public static string ErrorMessage(this ApiException ex)
        {
            return ex.GetContentAs<Error>().Message;
        }
    }
    public static class HttpApi
    {
        public static string AuthToken { get; set; }
        private const string ApiUrl = "http://srv.kemoke.net:2424/";
        public static IAuthApi Auth => RestService.For<IAuthApi>(ApiUrl+"auth");
        public static IChannelApi Channel => RestService.For<IChannelApi>(ApiUrl+"chat");
        public static IRoleApi Role => RestService.For<IRoleApi>(ApiUrl+"role");
        public static ISettingsApi Settings => RestService.For<ISettingsApi>(ApiUrl + "settings");
        public static ITeamApi Team => RestService.For<ITeamApi>(ApiUrl+"team");
        public static IUserApi User => RestService.For<IUserApi>(ApiUrl+"user");
    }

    public interface IAuthApi
    {
        [Post("/login")]
        Task<LoginResponse> LoginAsync([Body] LoginRequest body);

        [Post("/register")]
        Task<UserInfo> RegisterAsync([Body] RegisterRequest body);
    }

    public interface IChannelApi
    {
        
    }

    public interface IRoleApi
    {
        
    }

    public interface ITeamApi
    {
        
    }

    public interface IUserApi
    {
        
    }

    public interface ISettingsApi
    {
        
    }
}