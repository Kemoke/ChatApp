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
    public partial class Settings_Feature : FeatureFixture
    {
        private BrowserResponse loginResult;
        private BrowserResponse infoChangeResult;
        private BrowserResponse passwordChangeResult;
        private readonly FeaturesConfig config;
        private readonly User user;
        private User user2;

        #region Setup/Teardown

        public Settings_Feature(ITestOutputHelper output) : base(output)
        {

            config = new FeaturesConfig();

            user = DataGenerator.GenerateSingleUser(config.Context);
            
        }

        #endregion

        private void Given_the_user_is_logged_in()
        {
            loginResult = config.Browser.Post("/auth/register", with =>
            {
                with.BodyJson(new RegisterRequest { User = user });
                with.Accept(new MediaRange("application/json"));
            }).Result;

            loginResult = config.Browser.Post("/auth/login", with =>
            {
                with.BodyJson(new LoginRequest
                {
                    Username = user.Username,
                    Password = user.Password
                });
                with.Accept(new MediaRange("application/json"));
            }).Result;


            Assert.Equal(HttpStatusCode.OK, loginResult.StatusCode);
            var body = loginResult.BodyJson<LoginResponse>();
            Assert.Equal(body.User.Username, user.Username);
            Assert.NotNull(body.Token);
            Assert.NotEmpty(body.Token);
        }

        private void User_wants_to_change_his_personal_info()
        {
            user2 = DataGenerator.GenerateSingleUser(config.Context);

            infoChangeResult = config.Browser.Put("/user/", with =>
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
            }).Result;
        }

        private void Info_change_successful()
        {
            StepExecution.Current.Comment(infoChangeResult.BodyJson<Error>().Message);
            Assert.Equal("Data changed successfully", infoChangeResult.BodyJson<Error>().Message);
        }

        private void User_wants_to_change_his_password()
        {
            passwordChangeResult = config.Browser.Post("/user/change_password", with =>
            {
                with.BodyJson(new ChangePasswordRequest
                {
                    UserId = loginResult.BodyJson<LoginResponse>().User.Id,
                    NewPassword = "newPassword",
                    OldPassword = user.Password
                });
                with.Accept(new MediaRange("application/json"));
                with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
            }).Result;
        }

        private void Password_change_successful()
        {
            Assert.Equal("Password changed successfully", passwordChangeResult.BodyJson<Error>().Message);
        }
    }
}