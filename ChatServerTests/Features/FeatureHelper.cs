using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Model;
using ChatServer.Request;
using Nancy.Responses.Negotiation;
using Nancy.Testing;

namespace ChatServerTests.Features
{
    class FeatureHelper
    {
        private readonly FeaturesConfig config;

        public FeatureHelper(FeaturesConfig config)
        {
            this.config = config;
        }

        public async Task<BrowserResponse> LoginResponse(User user)
        {
            return await config.Browser.Post("/auth/login", with =>
            {
                with.BodyJson(new LoginRequest
                {
                    Username = user.Username,
                    Password = user.Password
                });
                with.Accept(new MediaRange("application/json"));
            });
        }

        public async Task<BrowserResponse> RegisterResponse(User user)
        {
            return await config.Browser.Post("/auth/register", with =>
            {
                with.BodyJson(new RegisterRequest {User = user});
                with.Accept(new MediaRange("application/json"));
            });
        }

        public async Task<BrowserResponse> CreateTeamResponse(Team team, dynamic token)
        {
            return await config.Browser.Post("/team/", with =>
            {
                with.BodyJson(new CreateTeamRequest
                {
                    Name = team.Name
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> CreateRoleResponse(Role role, dynamic token)
        {
            return await config.Browser.Post("/role/", with =>
            {
                with.BodyJson(new CreateRoleRequest
                {
                    Name = role.Name
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> AssignRoleResponse(int roleId, int teamId, int userId, dynamic token)
        {
            return await config.Browser.Post("/role/assign", with =>
            {
                with.BodyJson(new AssignRoleRequest
                {
                    TeamId = teamId,
                    RoleId = roleId,
                    UserId = userId

                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> CreateChannelResponse(Channel channel, int userId, dynamic token)
        {
            return await config.Browser.Post("/channel/", with =>
            {
                with.BodyJson(new CreateChannelRequest
                {
                    ChannelName = channel.ChannelName,
                    UserId = userId,
                    TeamId = channel.TeamId
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> RetrieveChannelListResponse(int teamId, dynamic token)
        {
            return await config.Browser.Get("/channel/", with =>
            {
                with.BodyJson(new ListChannelRequest
                {
                    TeamId = teamId
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> EditChannelResponse(int channelId, string name, dynamic token)
        {
            return await config.Browser.Put("/channel/" + channelId, with =>
            {
                with.BodyJson(new Channel
                {
                    ChannelName = name
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> DeleteChannelResponse(int channelId, dynamic token)
        {
            return await config.Browser.Delete("/channel/" + channelId, with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> GetChannelResponse(int channelId, dynamic token)
        {
            return await config.Browser.Get("/channel/" + channelId, with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> SendMessageResponse(string text, int targetId, int senderId, int channelId,
            dynamic token)
        {
            return await config.Browser.Post("/channel/send", with =>
            {
                with.BodyJson(new SendMessageRequest
                {
                    MessageText = "sdadadad",
                    TargetId = targetId,
                    SenderId = senderId,
                    ChannelId = channelId
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> RetrieveMessageListResponse(int targetId, int senderId, int channelId,
            dynamic token)
        {
            return await config.Browser.Get("/channel/messages/0/10", with =>
            {
                with.BodyJson(new GetMessagesRequest
                {
                    ChannelId = channelId,
                    SenderId = senderId,
                    TargetId = targetId
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> GetNewMessagesResponse(int channelId, int messageId, dynamic token)
        {
            return await config.Browser.Get("/channel/messages/new", with =>
            {
                with.BodyJson(new CheckNewMessagesRequest
                {
                    ChannelId = channelId,
                    MessageId = messageId
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> DeleteRoleResponse(int roleId, dynamic token)
        {
            return await config.Browser.Delete("/role/" + roleId, with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> EditRoleResponse(int roleId, dynamic token)
        {
            return await config.Browser.Put("/role/" + roleId, with =>
            {
                with.BodyJson(new EditRoleRequest
                {
                    RoleName = "Hello"
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> GetRoleResponse(int roleId, dynamic token)
        {
            return await config.Browser.Get("/role/" + roleId, with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> GetRoleListResponse(dynamic token)
        {
            return await config.Browser.Get("/role/", with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> UnsignRoleResponse(int roleId, int teamId, int userId, dynamic token)
        {
            return await config.Browser.Delete("/role/unsign", with =>
            {
                with.BodyJson(new UnsignRoleRequest
                {
                    TeamId = teamId,
                    RoleId = roleId,
                    UserId = userId

                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> UserInfoChangeResponse(User user, dynamic token)
        {
            return await config.Browser.Put("/user/", with =>
            {
                with.BodyJson(new EditUserInfoRequest
                {
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Company = user.Company,
                    Country = user.Country,
                    DateOfBirth = user.DateOfBirth,
                    PictureUrl = user.PictureUrl,
                    Gender = user.Gender
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> PasswordChangeResponse(int userId, string oldPassword, dynamic token)
        {
            return await config.Browser.Post("/user/change_password", with =>
            {
                with.BodyJson(new ChangePasswordRequest
                {
                    UserId = userId,
                    NewPassword = "newPassword",
                    OldPassword = oldPassword
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> GetSelfInfoResponse(dynamic token)
        {
            return await config.Browser.Get("/user/self", with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> GetUserInfoResponse(int userId, dynamic token)
        {
            return await config.Browser.Get("/user/" + userId, with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }

        public async Task<BrowserResponse> GetAllUsersResponse(dynamic token)
        {
            return await config.Browser.Get("/user/", with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", token);
            });
        }
    }
}
