using AngleSharp.Network.Default;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace ChatServerTests.Features
{
    [FeatureDescription(
        @"As an admin user I want to be able to create a channel inside of a team")]
    [Label("Story-4")]

    public partial class Chat_Feature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Chat")]
        public void Successfuly_create_channel()
        {
            /*Runner.RunScenario(
                Given_the_user_is_logged_in,
                Given_the_team_and_channel_inside_team_exist,
                User_sends_message,
                Message_is_sent_successfuly);*/
        }

        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Chat")]
        public void Upon_opening_user_wants_to_see_messages_for_channel()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                Given_the_team_and_channel_inside_team_exist,
                Given_that_messages_for_certain_channel_exist,
                Request_is_sent_to_retrieve_messages,
                Messages_are_retrieved_successfuly);
        }
    }
}