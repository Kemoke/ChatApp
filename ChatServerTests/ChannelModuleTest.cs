using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Bogus;
using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using Nancy.Testing;
using Xunit;

namespace ChatServerTests
{
    public class ChannelModuleTest : HttpTest
    {
        private readonly Token token;
        public ChannelModuleTest()
        {
            var response = Browser.Post("/user/login", with =>
            {
                    with.HttpRequest();
                    //with login body
            }).Result;
            token = response.Body.DeserializeJson<Token>();
        }
        [Fact]
        public void TestCreateChannelOk()
        {
            var channel = DataGenerator.GenerateSingleChannel(context);
            var response = Browser.Post("/chat/create_channel", with =>
            {
                with.HttpRequest();
                with.JsonBody(new CreateChannelRequest
                {
                    ChannelName = channel.ChannelName,
                    TeamId = channel.TeamId,
                    Token = token
                });
            }).Result;
            var responseChannel = response.Body.DeserializeJson<Channel>();
            Assert.Equal(channel.ChannelName, responseChannel.ChannelName);
            Assert.Equal(channel.TeamId, responseChannel.TeamId);
        }
        [Fact]
        public void TestCreateChannelInvalid()
        {
            var response = Browser.Post("/chat/create_channel", with =>
            {
                with.HttpRequest();
                with.JsonBody(new CreateChannelRequest
                {
                    Token = token
                });
            }).Result;
            var body = response.Body.DeserializeJson<Error>();
        }
    }
}