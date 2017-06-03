using LightBDD.Framework;
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
        [ScenarioCategory("Channel")]
        public void Successfuly_create_channel()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                Given_the_team_and_channel_inside_team_exist,
                User_sends_message,
                Message_is_sent_successfuly);
        }
    }
}