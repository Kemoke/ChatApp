using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace ChatServerTests.Features
{
    [FeatureDescription(
        @"As an admin user I want to be able to create a channel inside of a team")]
    [Label("Story-4")]


    public partial class ChannelFeature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Channel")]
        public Task Successfuly_create_channel()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_the_user_creates_team_and_is_admin,
                User_tries_to_create_new_channel_providing_channel_name,
                Channel_creation_successful);
        }


        [Scenario]
        [Label("Ticket-2")]
        [ScenarioCategory("Chanel")]
        public Task Unsuccessful_channel_creation_caused_by_duplicate_channel_name()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_the_user_creates_team_and_is_admin,
                User_tries_to_create_new_channel_providing_channel_name,
                User_tries_to_create_new_channel_providing_channel_name_that_already_exists,
                Channel_creation_unsuccessful);
        }

        [Scenario]
        [Label("Ticket-3")]
        [ScenarioCategory("Channel")]
        public Task When_a_user_enters_team_list_of_channels_should_be_displayed()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_the_user_creates_team_and_is_admin,
                Given_the_user_is_inside_of_a_team_and_there_exists_list_of_channels_in_database,
                Users_wants_to_see_list_of_all_channels_inside_of_that_team,
                List_retrieved_successfully);
        }

        [Scenario]
        [Label("Ticket-4")]
        [ScenarioCategory("Channel")]
        public Task Edit_channel_info()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_the_user_creates_team_and_is_admin,
                Users_wants_to_change_channel_name,
                Channel_name_change_successful);
        }


        [Scenario]
        [Label("Ticket-5")]
        [ScenarioCategory("Channel")]
        public Task Delete_channel()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_the_user_creates_team_and_is_admin,
                User_tries_to_create_new_channel_providing_channel_name,
                Users_tries_to_delete_that_channel,
                Channel_deleteion_successful);
        }


        [Scenario]
        [Label("Ticket-6")]
        [ScenarioCategory("Channel")]
        public Task Get_channel()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_the_user_creates_team_and_is_admin,
                User_tries_to_create_new_channel_providing_channel_name,
                Users_tries_to_retrieve_created_channel,
                Channel_retrieved_successfully);
        }

        [Scenario]
        [Label("Ticket-6")]
        [ScenarioCategory("Channel")]
        public Task User_tries_to_create_channel_with_non_admin_account()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_there_are_several_roles_in_database,
                User_tries_to_create_new_channel_providing_channel_name_with_non_admin_account,
                Channel_creation_failed);
        }
    }
}
