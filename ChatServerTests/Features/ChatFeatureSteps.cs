﻿using System.Collections.Generic;
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
        private BrowserResponse newMessageList;
        private BrowserResponse sendMessages;
        private BrowserResponse retrievedMessageList2;
        private BrowserResponse sendMessages2;
        


        #region Setup/Teardown

        public ChatFeature(ITestOutputHelper output) : base(output)
        {

            config = new FeaturesConfig();

            user1 = DataGenerator.GenerateSingleUser(config.Context);
            user2 = DataGenerator.GenerateSingleUser(config.Context);

        }

        #endregion

        private async Task Given_the_user_is_logged_in()
        {
            loginResult = await config.Browser.Post("/auth/register", with =>
            {
                with.Body(JsonConvert.SerializeObject(new RegisterRequest {User = user1}), "application/json");
                with.Accept(new MediaRange("application/json"));
            });
            loginResult = await config.Browser.Post("/auth/login", with =>
            {
                with.Body(JsonConvert.SerializeObject(new LoginRequest
                {
                    Username = user1.Username,
                    Password = user1.Password
                }), "application/json");
                with.Accept(new MediaRange("application/json"));
            });
            Assert.Equal(HttpStatusCode.OK, loginResult.StatusCode);
            var body = loginResult.Body.DeserializeJson<LoginResponse>();
            Assert.Equal(body.User.Username, user1.Username);
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
            sendMessageResult = await config.Browser.Post("/channel/send", with =>
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
                ;
        }

        private Task Message_is_sent_successfuly()
        {
            Assert.Equal(HttpStatusCode.OK, sendMessageResult.StatusCode);
            return Task.CompletedTask;
        }


        private async Task Given_that_messages_for_certain_channel_exist()
        {
            messages = DataGenerator.GenerateMessageList(config.Context, 10, channel.Id, user1.Id, user2.Id).ToList();

            foreach (var m in messages)
            {
                sendMessages = await config.Browser.Post("/channel/send", with =>
                    {
                        with.BodyJson(new SendMessageRequest
                        {
                            MessageText = m.MessageText,
                            SenderId = m.SenderId,
                            TargetId = m.TargetId,
                            ChannelId = m.ChannelId
                        });
                        with.Accept(new MediaRange("application/json"));
                        with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                    });
            }
        }

        private async Task Request_is_sent_to_retrieve_messages()
        {
            retrievedMessageList = await config.Browser.Get("/channel/messages/0/10", with =>
                {
                    with.Body(JsonConvert.SerializeObject(new GetMessagesRequest()
                    {
                        ChannelId = channel.Id,
                        SenderId = user1.Id,
                        TargetId = user2.Id
                    }), "application/json");
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.Body.DeserializeJson<LoginResponse>().Token);
                });
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
                sendMessages2 = await config.Browser.Post("/channel/send", with =>
                    {
                        with.BodyJson(new SendMessageRequest
                        {
                            MessageText = m.MessageText,
                            SenderId = m.SenderId,
                            TargetId = m.TargetId,
                            ChannelId = sendMessages.BodyJson<Message>().ChannelId
                        });
                        with.Accept(new MediaRange("application/json"));
                        with.Header("Authorization", loginResult.BodyJson<LoginResponse>().Token);
                    });
            }
        }

        private async Task Request_is_sent_to_retrieve_new_messages()
        {
            newMessageList = await config.Browser.Get("/channel/messages/new", with =>
                {
                    with.Body(JsonConvert.SerializeObject(new CheckNewMessagesRequest
                    {
                        ChannelId = sendMessages.BodyJson<Message>().ChannelId,
                        MessageId = sendMessages.BodyJson<Message>().Id
                    }), "application/json");
                    with.Accept(new MediaRange("application/json"));
                    with.Header("Authorization", loginResult.Body.DeserializeJson<LoginResponse>().Token);
                });
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