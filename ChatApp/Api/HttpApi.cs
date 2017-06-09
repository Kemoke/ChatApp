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

    public interface IChannelApi
    {
        [Get("/")]
        Task<List<Channel>> GetListAsync([Body] ListChannelRequest request);

        [Get("/{id}")]
        Task<Channel> GetAsync(int id);

        [Post("/")]
        Task<Channel> SaveAsync([Body] Channel body);

        [Put("/{id}")]
        Task<Channel> EditAsync(int id, [Body] Channel body);

        [Delete("/{id}")]
        Task<string> DeleteAsync(int id);

        [Post("/send")]
        Task<Message> SendMessageAsync([Body] SendMessageRequest request);

        [Get("/messages/{skip}/{limit}")]
        Task<List<Message>> GetMessagesAsync([Body] GetMessagesRequest request, int skip, int limit);

        [Get("/messages/new")]
        Task<List<Message>> GetNewMessagesAsync([Body] CheckNewMessagesRequest request);
    }

    public interface IRoleApi
    {
        [Get("/")]
        Task<List<Role>> GetListAsync();

        [Get("/{id}")]
        Task<Role> GetAsync(int id);

        [Post("/")]
        Task<Role> SaveAsync([Body] Role body);

        [Put("/{id}")]
        Task<Role> EditAsync(int id, [Body] Role body);

        [Delete("/{id}")]
        Task<string> DeleteAsync(int id);

        [Post("/assign")]
        Task<Role> AssignRoleAsync([Body] AssignRoleRequest request);
    }

    public interface ITeamApi
    {
        [Get("/")]
        Task<List<Team>> GetListAsync();

        [Get("/{id}")]
        Task<Team> GetAsync(int id);

        [Post("/")]
        Task<Team> SaveAsync([Body] Team body);

        [Put("/{id}")]
        Task<Team> EditAsync(int id, [Body] Team body);
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