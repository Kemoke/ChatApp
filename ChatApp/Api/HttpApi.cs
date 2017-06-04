using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public interface ICrudApi<T>
    {
        [Get("/")]
        Task<List<T>> GetListAsync();

        [Get("/{id}")]
        Task<T> GetAsync(int id);

        [Post("/")]
        Task<T> SaveAsync([Body] T body);

        [Put("/{id}")]
        Task<T> EditAsync(int id, [Body] T body);

        [Delete("/{id}")]
        Task<string> DeleteAsync(int id);
    }

    public interface IChannelApi : ICrudApi<Channel>
    {
        [Post("/send")]
        Task<Message> SendMessageAsync([Body] SendMessageRequest request);

        [Get("/messages/{skip}/{limit}")]
        Task<List<Message>> GetMessagesAsync(int skip, int limit);

        [Get("/messages/new")]
        Task<List<Message>> GetNewMessagesAsync();
    }

    public interface IRoleApi : ICrudApi<Role>
    {
        [Post("/assign")]
        Task<Role> AssignRoleAsync([Body] AssignRoleRequest request);
    }

    public interface ITeamApi : ICrudApi<Team>
    {
        
    }

    public interface IUserApi
    {
        [Get("/")]
        Task<List<User>> GetListAsync();

        [Get("/{id}")]
        Task<UserInfo> GetAsync(int id);

        [Get("/self")]
        Task<UserInfo> GetSelfAsync();

        [Put("/")]
        Task<string> EditInfoAsync([Body] EditUserInfoRequest request);

        [Post("/change_password")]
        Task<string> ChangePasswordAsync([Body] ChangePasswordRequest request);
    }
}