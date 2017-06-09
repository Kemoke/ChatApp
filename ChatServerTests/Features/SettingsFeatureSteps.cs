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
        private readonly FeatureHelper helper;
        private BrowserResponse loginResult;
        private BrowserResponse userInfoChangeResult;
        private BrowserResponse selfInfoResult;
        private BrowserResponse passwordChangeResult;
        private BrowserResponse failedPasswordChangeResult;
        private BrowserResponse userInfoResult;
        private BrowserResponse allUsersResult;
        private BrowserResponse registerResult;
        private readonly FeaturesConfig config;
        private readonly User user;
        private User user2;
        private List<User> users;

        #region Setup/Teardown

        public SettingsFeature(ITestOutputHelper output) : base(output)
        {
            config = new FeaturesConfig();

            user = DataGenerator.GenerateSingleUser(config.Context);

            helper = new FeatureHelper(config);
        }

        #endregion

        private async Task Given_the_user_is_logged_in()
        {
            registerResult = await helper.RegisterResponse(user);

            loginResult = await helper.LoginResponse(user);


            Assert.Equal(HttpStatusCode.OK, loginResult.StatusCode);
            var body = loginResult.BodyJson<LoginResponse>();
            Assert.Equal(body.User.Username, user.Username);
            Assert.NotNull(body.Token);
            Assert.NotEmpty(body.Token);
        }

        private async Task User_wants_to_change_his_personal_info()
        {
            user2 = DataGenerator.GenerateSingleUser(config.Context);

            userInfoChangeResult = await helper.UserInfoChangeResponse(user2, loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task Info_change_successful()
        {
            StepExecution.Current.Comment(userInfoChangeResult.BodyJson<Msg>().Message);
            Assert.Equal("Data changed successfully", userInfoChangeResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }

        private async Task User_wants_to_change_his_password()
        {
            passwordChangeResult = await helper.PasswordChangeResponse(loginResult.BodyJson<LoginResponse>().User.Id,
                user.Password, loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task Password_change_successful()
        {
            Assert.Equal("Password changed successfully", passwordChangeResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }

        private async Task User_wants_to_change_his_password_and_has_provided_wrong_old_password()
        {
            failedPasswordChangeResult = await helper.PasswordChangeResponse(loginResult.BodyJson<LoginResponse>().User.Id,
                user.Password + "das", loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task Password_change_unsuccessful()
        {
            Assert.Equal(HttpStatusCode.BadRequest, failedPasswordChangeResult.StatusCode);
            Assert.Equal("Wrong input for old password", failedPasswordChangeResult.BodyJson<Msg>().Message);
            return Task.CompletedTask;
        }

        private async Task User_wants_to_retrieve_info_about_himself()
        {
            selfInfoResult = await helper.GetSelfInfoResponse(loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task Self_info_retrieval_successful()
        {
            Assert.Equal(loginResult.BodyJson<LoginResponse>().User.Id, selfInfoResult.BodyJson<User>().Id);
            return Task.CompletedTask;
        }

        private async Task User_wants_to_retrieve_info_about_certain_user()
        {
            userInfoResult = await helper.GetUserInfoResponse(loginResult.BodyJson<LoginResponse>().User.Id,
                loginResult.BodyJson<LoginResponse>().Token);
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
                await helper.RegisterResponse(u);
            }
        }

        private async Task User_wants_to_retrieve_list_containing_all_users()
        {
            allUsersResult = await helper.GetAllUsersResponse(loginResult.BodyJson<LoginResponse>().Token);
        }

        private Task List_retrieved_successfully()
        {
            Assert.NotEmpty(allUsersResult.BodyJson<List<User>>());
            return Task.CompletedTask;
        }
    }
}