using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatServer.Model;
using ChatServer.Request;
using ChatServer.Response;
using LightBDD.XUnit2;
using Nancy;
using Nancy.Responses.Negotiation;
using Nancy.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace ChatServerTests.Features
{
    public partial class ChatFeature : FeatureFixture
    {
        private User user;
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
        private BrowserResponse newMessageList;
        private BrowserResponse sendMessages;
        private BrowserResponse retrievedMessageList2;
        private BrowserResponse sendMessages2;
        private BrowserResponse registerResult;
        private readonly FeatureHelper helper;



        #region Setup/Teardown

        public ChatFeature(ITestOutputHelper output) : base(output)
        {

            config = new FeaturesConfig();

            user = DataGenerator.GenerateSingleUser(config.Context);
            user2 = DataGenerator.GenerateSingleUser(config.Context);

            helper = new FeatureHelper(config);
        }

        #endregion

        private async Task Given_the_user_is_logged_in()
        {
            registerResult = await helper.RegisterResponse(user);
            loginResult = await helper.LoginResponse(user);


            Assert.Equal(HttpStatusCode.OK, loginResult.StatusCode);
            var body = loginResult.Body.DeserializeJson<LoginResponse>();
            Assert.Equal(body.User.Username, user.Username);
            Assert.NotNull(body.Token);
            Assert.NotEmpty(body.Token);
        }

        private Task Given_the_team_and_channel_inside_team_exist()
        {
            team = DataGenerator.GenerateSingleTeam(config.Context);

            channel = DataGenerator.GenerateSingleChannel(config.Context, team.Id);
            return Task.CompletedTask;
        }

        private async Task User_sends_message()
        {
            sendMessageResult = await helper.SendMessageResponse("sdsada", user.Id, user2.Id, channel.Id, loginResult.Body.DeserializeJson<LoginResponse>().Token);
        }

        private Task Message_is_sent_successfuly()
        {
            Assert.Equal(HttpStatusCode.OK, sendMessageResult.StatusCode);
            return Task.CompletedTask;
        }


        private async Task Given_that_messages_for_certain_channel_exist()
        {
            messages = DataGenerator.GenerateMessageList(config.Context, 10, channel.Id, user.Id, user2.Id).ToList();

            foreach (var m in messages)
            {
                sendMessages = await helper.SendMessageResponse(m.MessageText, m.TargetId, m.SenderId, m.ChannelId, loginResult.Body.DeserializeJson<LoginResponse>().Token);
            }
        }

        private async Task Request_is_sent_to_retrieve_messages()
        {
            retrievedMessageList = await helper.RetrieveMessageListResponse(user2.Id, user.Id, channel.Id,
                loginResult.Body.DeserializeJson<LoginResponse>().Token);
        }

        private Task Messages_are_retrieved_successfuly()
        {
            Assert.NotEmpty(retrievedMessageList.Body.DeserializeJson<List<Message>>());
            return Task.CompletedTask;
        }

        private async Task New_messages_were_sent()
        {
            foreach (var m in messages)
            {
                sendMessages2 = await helper.SendMessageResponse(m.MessageText, m.TargetId, m.SenderId,
                    sendMessages.BodyJson<Message>().ChannelId,
                    loginResult.Body.DeserializeJson<LoginResponse>().Token);
            }
        }

        private async Task Request_is_sent_to_retrieve_new_messages()
        {
            newMessageList = await helper.GetNewMessagesResponse(sendMessages.BodyJson<Message>().ChannelId,
                sendMessages.BodyJson<Message>().Id, loginResult.Body.DeserializeJson<LoginResponse>().Token);
        }

        private Task New_messages_are_retrieved_successfuly()
        {
            Assert.NotEmpty(newMessageList.BodyJson<List<Message>>());
            return Task.CompletedTask;
        }

       
        private Task No_new_messages_are_retrieved()
        {
            Assert.Empty(newMessageList.BodyJson<List<Message>>());
            return Task.CompletedTask;
        }

    }
}