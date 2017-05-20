using System.Linq;
using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using Nancy;
using Nancy.Json;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace ChatServerTests
{
    public class AuthModuleTest : HttpTest
    {
        private readonly ITestOutputHelper output;
        private readonly User user;
        public AuthModuleTest(ITestOutputHelper output)
        {
            this.output = output;
            user = DataGenerator.GenerateSingleUser(Context);
        }

        [Fact]
        public void TestRegister()
        {
            var result = Browser.Post("/auth/register", with =>
            {
                with.Body(JsonConvert.SerializeObject(new RegisterRequest{User = user}), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            output.WriteLine(result.Body.AsString());
            Assert.Equal(result.StatusCode, HttpStatusCode.OK);
        }

        [Fact]
        public void TestRegisterExists()
        {
            var result = Browser.Post("/auth/register", with =>
            {
                with.Body(JsonConvert.SerializeObject(new RegisterRequest { User = user }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            output.WriteLine(result.Body.AsString());
            result = Browser.Post("/auth/register", with =>
            {
                with.Body(JsonConvert.SerializeObject(new RegisterRequest { User = user }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            output.WriteLine(result.Body.AsString());
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            var response = JsonConvert.DeserializeObject<Error>(result.Body.AsString());
            Assert.Equal("Email already exists", response.Message);
        }

        [Fact]
        public void TestLogin()
        {
            var result = Browser.Post("/auth/register", with =>
            {
                with.Body(JsonConvert.SerializeObject(new RegisterRequest { User = user }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            output.WriteLine(result.Body.AsString());
            result = Browser.Post("/auth/login", with =>
            {
                with.Body(JsonConvert.SerializeObject(new LoginRequest
                {
                    Username = user.Username,
                    Password = user.Password
                }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            output.WriteLine(result.Body.AsString());
            Assert.Equal(result.StatusCode, HttpStatusCode.OK);
            var body = result.Body.DeserializeJson<LoginResponse>();
            Assert.Equal(body.User.Username, user.Username);
            Assert.NotNull(body.Token);
            Assert.NotEmpty(body.Token);
        }

        [Fact]
        public void TestLoginInvalidUsername()
        {
            var result = Browser.Post("/auth/login", with =>
            {
                with.Body(JsonConvert.SerializeObject(new LoginRequest
                {
                    Username = user.Username,
                    Password = user.Password
                }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            output.WriteLine(result.Body.AsString());
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            var response = JsonConvert.DeserializeObject<Error>(result.Body.AsString());
            Assert.Equal("Invalid credentials", response.Message);
        }

        [Fact]
        public void TestLoginInvalidPassword()
        {
            var result = Browser.Post("/auth/register", with =>
            {
                with.Body(JsonConvert.SerializeObject(new RegisterRequest { User = user }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            output.WriteLine(result.Body.AsString());
            result = Browser.Post("/auth/login", with =>
            {
                with.Body(JsonConvert.SerializeObject(new LoginRequest
                {
                    Username = user.Username,
                    Password = "asda"
                }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            output.WriteLine(result.Body.AsString());
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
            var response = JsonConvert.DeserializeObject<Error>(result.Body.AsString());
            Assert.Equal("Invalid credentials", response.Message);
        }
    }
}