using System;
using System.Collections.Generic;
using System.Text;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace ChatServerTests.Features
{
    [FeatureDescription(
        @"As an admin user I want to be able to create a channel inside of a team")]
    [Label("Story-4")]


    public partial class Create_Channel_Feature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Channel")]
        public void Successfuly_create_channel()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                Given_the_user_creates_team_and_is_admin,
                User_tries_to_create_new_channel_providing_channel_name,
                Channel_creation_successful);
        }


        [Scenario]
        [Label("Ticket-2")]
        [ScenarioCategory("Channel")]
        public void Unsuccessful_channel_creation_caused_by_duplicate_channel_name()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                Given_the_user_creates_team_and_is_admin,
                User_tries_to_create_new_channel_providing_channel_name,
                User_tries_to_create_new_channel_providing_channel_name_that_already_exists,
                Channel_creation_unsuccessful);
        }
    }
}
