using System.Threading.Tasks;
using AngleSharp.Network.Default;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace ChatServerTests.Features
{
    [FeatureDescription(
        @"As an admin user I want to be able to create a channel inside of a team")]
    [Label("Story-5")]

    public partial class ChatFeature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Chat")]
        public Task Successfuly_create_channel()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_the_team_and_channel_inside_team_exist,
                User_sends_message,
                Message_is_sent_successfuly);
        }

        [Scenario]
        [Label("Ticket-2")]
        [ScenarioCategory("Chat")]
        public Task Upon_opening_user_wants_to_see_messages_for_channel()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_the_team_and_channel_inside_team_exist,
                Given_that_messages_for_certain_channel_exist,
                Request_is_sent_to_retrieve_messages,
                Messages_are_retrieved_successfuly);
        }

        [Scenario]
        [Label("Ticket-3")]
        [ScenarioCategory("Chat")]
        public Task User_tries_to_recieve_new_messages()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_the_team_and_channel_inside_team_exist,
                Given_that_messages_for_certain_channel_exist,
                New_messages_were_sent,
                Request_is_sent_to_retrieve_new_messages,
                New_messages_are_retrieved_successfuly);
        }

        [Scenario]
        [Label("Ticket-3")]
        [ScenarioCategory("Chat")]
        public Task User_tries_to_recieve_new_messages_but_there_are_no_messages_in_channel()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_the_team_and_channel_inside_team_exist,
                Given_that_messages_for_certain_channel_exist,
                Request_is_sent_to_retrieve_new_messages,
                No_new_messages_are_retrieved);
        }
    }
}