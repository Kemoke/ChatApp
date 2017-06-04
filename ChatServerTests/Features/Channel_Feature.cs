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


    public partial class Channel_Feature
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

        [Scenario]
        [Label("Ticket-3")]
        [ScenarioCategory("Channel")]
        public void When_a_user_enters_team_list_of_channels_should_be_displayed()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                Given_the_user_is_inside_of_a_team,
                Users_wants_to_see_list_of_all_channels_inside_of_that_team,
                List_retrieved_successfully);
        }

        [Scenario]
        [Label("Ticket-4")]
        [ScenarioCategory("Channel")]
        public void Edit_channel_info()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                Given_the_user_creates_team_and_is_admin,
                Users_wants_to_change_channel_name,
                Channel_name_change_successful);
        }
    }
}
