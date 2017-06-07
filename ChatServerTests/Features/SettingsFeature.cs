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
        @"As an user I want to be able to change my personal information.
          Also I would like to retrieve information about users.")]
    [Label("Story-6")]

    public partial class SettingsFeature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Settings")]
        public Task Personal_info_chage()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                User_wants_to_change_his_personal_info,
                Info_change_successful);
        }


        [Scenario]
        [Label("Ticket-2")]
        [ScenarioCategory("Settings")]
        public Task Password_change()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                User_wants_to_change_his_password,
                Password_change_successful);
        }

        [Scenario]
        [Label("Ticket-3")]
        [ScenarioCategory("Settings")]
        public Task Password_change_attempt_with_wrong_old_password()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                User_wants_to_change_his_password_and_has_provided_wrong_old_password,
                Password_change_unsuccessful);
        }

        [Scenario]
        [Label("Ticket-4")]
        [ScenarioCategory("Settings")]
        public Task Retriving_information_about_logged_in_user()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                User_wants_to_retrieve_info_about_himself,
                Self_info_retrieval_successful);
        }

        [Scenario]
        [Label("Ticket-5")]
        [ScenarioCategory("Settings")]
        public Task Retriving_information_about_user()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                User_wants_to_retrieve_info_about_certain_user,
                Info_retrieval_successful);
        }

        [Scenario]
        [Label("Ticket-6")]
        [ScenarioCategory("Settings")]
        public Task Retrieving_all_users()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_there_are_registered_users_in_database,
                User_wants_to_retrieve_list_containing_all_users,
                List_retrieved_successfully);
        }
    }
}
