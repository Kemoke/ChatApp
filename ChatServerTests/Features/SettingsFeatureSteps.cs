using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using LightBDD.Framework;
using LightBDD.Framework.Commenting;
using LightBDD.XUnit2;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Xunit;
using Xunit.Abstractions;

namespace ChatServerTests.Features
{
    public partial class SettingsFeature : FeatureFixture
    {
        private BrowserResponse loginResult;
        private BrowserResponse infoChangeResult;
        private BrowserResponse selfInfoResult;
        private BrowserResponse passwordChangeResult;
        private BrowserResponse failedPasswordChangeResult;
        private BrowserResponse userInfoResult;
        private BrowserResponse allUsersResult;
        private readonly FeaturesConfig config;
        private readonly User user;
        private User user2;
        private List<User> users;

        #region Setup/Teardown

        public SettingsFeature(ITestOutputHelper output) : base(output)
        {

            config = new FeaturesConfig();
            user = DataGenerator.GenerateSingleUser(config.Context);
            
        }

        #endregion

        private async Task Given_the_user_is_logged_in()
        {
            loginResult = await config.Browser.Post("/auth/register", with =>
            {
                with.BodyJson(new RegisterRequest { User = user });
                with.Accept(new MediaRange("application/json"));
            });

            loginResult = await config.Browser.Post("/auth/login", with =>
            {
                with.BodyJson(new LoginRequest
                {
                    Username = user.Username,
                    Password = user.Password
                });
                with.Accept(new MediaRange("application/json"));
            });


            Assert.Equal(HttpStatusCode.OK, loginResult.StatusCode);
            var body = loginResult.BodyJson<LoginResponse>();
            Assert.Equal(body.User.Username, user.Username);
            Assert.NotNull(body.Token);
            Assert.NotEmpty(body.Token);
        }

        private async Task User_wants_to_change_his_personal_info()
        {
            user2 = DataGenerator.GenerateSingleUser(config.Context);

            infoChangeResult = await config.Browser.Put("/user/", with =>
            {
                with.BodyJson(new EditUserInfoRequest
                {
                    Username = user2.Username,
                    FirstName = user2.FirstName,
                    LastName = user2.LastName,
                    Company = user2.Company,
                    Country = user2.Country,
                    DateOfBirth = user2.DateOfBirth,
                    PictureUrl = user2.PictureUrl,
                    Gender = user2.Gender
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Info_change_successful()
        {
            StepExecution.Current.Comment(infoChangeResult.BodyJson<Msg>().Message);
            Assert.Equal("Data changed successfully", infoChangeResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }

        private async Task User_wants_to_change_his_password()
        {
            passwordChangeResult = await config.Browser.Post("/user/change_password", with =>
            {
                with.BodyJson(new ChangePasswordRequest
                {
                    UserId = loginResult.BodyJson<LoginResponse>().User.Id,
                    NewPassword = "newPassword",
                    OldPassword = user.Password
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Password_change_successful()
        {
            Assert.Equal("Password changed successfully", passwordChangeResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }

        private async Task User_wants_to_change_his_password_and_has_provided_wrong_old_password()
        {
            failedPasswordChangeResult = await config.Browser.Post("/user/change_password", with =>
            {
                with.BodyJson(new ChangePasswordRequest
                {
                    UserId = loginResult.BodyJson<LoginResponse>().User.Id,
                    NewPassword = "newPassword",
                    OldPassword = "bing"
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Password_change_unsuccessful()
        {
            Assert.Equal(HttpStatusCode.BadRequest, failedPasswordChangeResult.StatusCode);
            Assert.Equal("Wrong input for old password", failedPasswordChangeResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }

        private async Task User_wants_to_retrieve_info_about_himself()
        {
            selfInfoResult = await config.Browser.Get("/user/self", with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Self_info_retrieval_successful()
        {
            Assert.Equal(loginResult.BodyJson<LoginResponse>().User.Id, selfInfoResult.BodyJson<User>().Id);
            return Task.CompletedTask;
        }

        private async Task User_wants_to_retrieve_info_about_certain_user()
        {
            userInfoResult = await config.Browser.Get("/user/"+loginResult.BodyJson<LoginResponse>().User.Id, with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task Info_retrieval_successful()
        {
            Assert.Equal(loginResult.BodyJson<LoginResponse>().User.FirstName, userInfoResult.BodyJson<UserInfo>().FirstName);
            return Task.CompletedTask;
        }

        private async Task Given_there_are_registered_users_in_database()
        {
            users = DataGenerator.GenerateUserList(config.Context, 9).ToList();

            foreach (var u in users)
            {
                await config.Browser.Post("/auth/register", with =>
                {
                    with.BodyJson(new RegisterRequest { User = u });
                    with.Accept(new MediaRange("application/json"));
                });
            }
        }

        private async Task User_wants_to_retrieve_list_containing_all_users()
        {
            allUsersResult = await config.Browser.Get("/user/", with =>
            {
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            });
        }

        private Task List_retrieved_successfully()
        {
            Assert.NotEmpty(allUsersResult.BodyJson<List<User>>());
            return Task.CompletedTask;
        }
    }
}