using System.Collections.Generic;
using System.Linq;
using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using LightBDD.Framework;
using LightBDD.Framework.Commenting;
using LightBDD.XUnit2;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace ChatServerTests.Features
{
    public partial class Chat_Feature : FeatureFixture
    {
        private User user1;
        private User user2;
        private List<Message> messages;
        private Channel channel;
        private Team team;
        private readonly FeaturesConfig config;
        private BrowserResponse retrievedMessageList;
        private BrowserResponse createTeamResult;
        private BrowserResponse addRoleResult;
        private BrowserResponse sendMessageResult;
        private BrowserResponse createChannelResult;
        private BrowserResponse createNewChannelResult;
        private BrowserResponse loginResult;



        #region Setup/Teardown

        public Chat_Feature(ITestOutputHelper output) : base(output)
        {

            config = new FeaturesConfig();

            user1 = DataGenerator.GenerateSingleUser(config.Context);
            user2 = DataGenerator.GenerateSingleUser(config.Context);

        }
        #endregion

        private void Given_the_user_is_logged_in()
        {
            loginResult = config.Browser.Post("/auth/register", with =>
            {
                with.Body(JsonConvert.SerializeObject(new RegisterRequest { User = user1 }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            loginResult = config.Browser.Post("/auth/login", with =>
            {
                with.Body(JsonConvert.SerializeObject(new LoginRequest
                {
                    Username = user1.Username,
                    Password = user1.Password
                }), "application/json");
                with.Accept(new MediaRange("application/json"));
            }).Result;
            Assert.Equal(HttpStatusCode.OK, loginResult.StatusCode);
            var body = loginResult.Body.DeserializeJson<LoginResponse>();
            Assert.Equal(body.User.Username, user1.Username);
            Assert.NotNull(body.Token);
            Assert.NotEmpty(body.Token);
        }

        private void Given_the_team_and_channel_inside_team_exist()
        {
            team = DataGenerator.GenerateSingleTeam(config.Context);

            channel = DataGenerator.GenerateSingleChannel(config.Context, team.Id);
        }

        private void User_sends_message()
        {
            sendMessageResult = config.Browser.Post("/channel/send", with =>
                {
                    with.Body(JsonConvert.SerializeObject(new SendMessageRequest()
                    {
                        MessageText = "sdadadad",
                        TargetId = user1.Id,
                        SenderId = user2.Id,
                        ChannelId = channel.Id
                    }), "application/json");
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.Body.DeserializeJson<LoginResponse>().Token);
                })
                .Result;
        }

        private void Message_is_sent_successfuly()
        {
            Assert.Equal(HttpStatusCode.OK, sendMessageResult.StatusCode);
        }


        private void Given_that_messages_for_certain_channel_exist()
        {
            messages = DataGenerator.GenerateMessageList(config.Context, 10, channel.Id, user1.Id, user2.Id).ToList();
        }

        private void Request_is_sent_to_retrieve_messages()
        {
            retrievedMessageList = config.Browser.Get("/channel/messages/0/10", with =>
                {
                    with.Body(JsonConvert.SerializeObject(new GetMessagesRequest()
                    {
                        ChannelId = channel.Id,
                        SenderId = user1.Id,
                        TargetId = user2.Id
                    }), "application/json");
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.Body.DeserializeJson<LoginResponse>().Token);
                })
                .Result;
        }

        private void Messages_are_retrieved_successfuly()
        {
            Assert.Equal(messages.Count, retrievedMessageList.Body.DeserializeJson<List<Message>>().Count);
        }
    }
}