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
        public static User LoggedInUser { get; set; }
        public static Team SelectedTeam { get; set; }
        public const string ApiUrl = "http://srv.kemoke.net:2424/";
        public static IAuthApi Auth => RestService.For<IAuthApi>(ApiUrl+"auth");
        public static IChannelApi Channel => RestService.For<IChannelApi>(ApiUrl+"channel");
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

    public interface IChannelApi
    {
        [Get("/")]
        Task<List<Channel>> GetListAsync([Body] ListChannelRequest request, [Header("Authorization")] string token);

        [Get("/{id}")]
        Task<Channel> GetAsync(int id, [Header("Authorization")] string token);

        [Post("/")]
        Task<Channel> SaveAsync([Body] CreateChannelRequest body, [Header("Authorization")] string token);

        [Put("/{id}")]
        Task<Channel> EditAsync(int id, [Body] Channel body, [Header("Authorization")] string token);

        [Delete("/{id}")]
        Task<string> DeleteAsync(int id, [Header("Authorization")] string token);

        [Post("/send")]
        Task<Message> SendMessageAsync([Body] SendMessageRequest request, [Header("Authorization")] string token);

        [Get("/messages/{skip}/{limit}")]
        Task<List<Message>> GetMessagesAsync([Body] GetMessagesRequest request, int skip, int limit, [Header("Authorization")] string token);

        [Get("/messages/new")]
        Task<List<Message>> GetNewMessagesAsync([Body] CheckNewMessagesRequest request, [Header("Authorization")] string token);
    }

    public interface IRoleApi
    {
        [Get("/")]
        Task<List<Role>> GetListAsync([Header("Authorization")] string token);

        [Get("/{id}")]
        Task<Role> GetAsync(int id, [Header("Authorization")] string token);

        [Post("/")]
        Task<Role> SaveAsync([Body] Role body, [Header("Authorization")] string token);

        [Put("/{id}")]
        Task<Role> EditAsync(int id, [Body] Role body, [Header("Authorization")] string token);

        [Delete("/{id}")]
        Task<string> DeleteAsync(int id, [Header("Authorization")] string token);

        [Post("/assign")]
        Task<Role> AssignRoleAsync([Body] AssignRoleRequest request, [Header("Authorization")] string token);
    }

    public interface ITeamApi
    {
        [Get("/")]
        Task<List<Team>> GetListAsync([Header("Authorization")] string token);

        [Get("/{id}")]
        Task<Team> GetAsync(int id, [Header("Authorization")] string token);

        [Post("/")]
        Task<Team> SaveAsync([Body] Team body, [Header("Authorization")] string token);

        [Put("/{id}")]
        Task<Team> EditAsync(int id, [Body] Team body, [Header("Authorization")] string token);
    }

    public interface IUserApi
    {
        [Get("/")]
        Task<List<User>> GetListAsync([Header("Authorization")] string token);

        [Get("/{id}")]
        Task<UserInfo> GetAsync(int id, [Header("Authorization")] string token);

        [Get("/self")]
        Task<UserInfo> GetSelfAsync([Header("Authorization")] string token);

        [Put("/")]
        Task<string> EditInfoAsync([Body] EditUserInfoRequest request, [Header("Authorization")] string token);

        [Post("/change_password")]
        Task<string> ChangePasswordAsync([Body] ChangePasswordRequest request, [Header("Authorization")] string token);
    }
}