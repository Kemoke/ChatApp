using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace ChatServerTests.Features
{
    [FeatureDescription(
        @"As a user I want to be able to create a channel")]
    [Label("Story-3")]
    public partial class Create_Team_Feature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Channel")]
        public void Successfuly_create_team()
        {
            Runner.RunScenario(

                Given_the_user_is_logged_in,
                User_tries_to_create_new_team_providing_team_name,
                Team_creation_successful);
        }

    }
}