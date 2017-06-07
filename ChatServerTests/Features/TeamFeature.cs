using System.Threading.Tasks;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace ChatServerTests.Features
{
    [FeatureDescription(
        @"As a user I want to be able to create a team")]
    [Label("Story-3")]
    public partial class TeamFeature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Teams")]
        public Task Successfuly_create_team()
        {
            return Runner.RunScenarioAsync(

                Given_the_user_is_logged_in,
                User_tries_to_create_new_team_providing_team_name,
                Team_creation_successful);
        }

        [Scenario]
        [Label("Ticket-2")]
        [ScenarioCategory("Teams")]
        public Task Wrong_team_name()
        {
            return Runner.RunScenarioAsync(

                Given_the_user_is_logged_in,
                Given_there_exists_a_team_in_the_database,
                User_tries_to_create_new_team_providing_team_name,
                Team_creation_unsuccessful);
        }


        [Scenario]
        [Label("Ticket-3")]
        [ScenarioCategory("Teams")]
        public Task User_wants_to_retrieve_list_of_teams_in_which_he_is_in()
        {
            return Runner.RunScenarioAsync(

                Given_the_user_is_logged_in,
                Given_there_exists_a_team_in_the_database,
                User_tries_to_retrieve_list_of_teams_in_which_he_is_in,
                List_retrieved_successfully);
        }

        [Scenario]
        [Label("Ticket-4")]
        [ScenarioCategory("Teams")]
        public Task User_wants_to_retrieve_info_about_team()
        {
            return Runner.RunScenarioAsync(

                Given_the_user_is_logged_in,
                Given_there_exists_a_team_in_the_database,
                User_tries_to_retrieve_about_the_team,
                Info_retrieved_successfully);
        }

        [Scenario]
        [Label("Ticket-5")]
        [ScenarioCategory("Teams")]
        public Task User_wants_to_edit_team_info()
        {
            return Runner.RunScenarioAsync(

                Given_the_user_is_logged_in,
                Given_there_exists_a_team_in_the_database,
                User_tries_to_edit_team_info,
                Info_edited_successfully);
        }

        [Scenario]
        [Label("Ticket-6")]
        [ScenarioCategory("Teams")]
        public Task User_wants_to_add_other_user_to_team()
        {
            return Runner.RunScenarioAsync(

                Given_the_user_is_logged_in,
                Given_there_exists_a_team_in_the_database,
                Given_another_user_is_registered,
                Role_is_being_created,
                User_tries_to_add_other_user_to_the_team,
                Adding_user_successful);
        }

        [Scenario]
        [Label("Ticket-7")]
        [ScenarioCategory("Teams")]
        public Task User_wants_to_remove_other_user_from_team()
        {
            return Runner.RunScenarioAsync(

                Given_the_user_is_logged_in,
                Given_there_exists_a_team_in_the_database,
                Given_another_user_is_registered,
                Role_is_being_created,
                User_tries_to_add_other_user_to_the_team,
                User_tries_to_remove_another_user_from_team,
                Remove_user_successful);
        }

        [Scenario]
        [Label("Ticket-8")]
        [ScenarioCategory("Teams")]
        public Task User_wants_to_delete_channel()
        {
            return Runner.RunScenarioAsync(

                Given_the_user_is_logged_in,
                Given_there_exists_a_team_in_the_database,
                User_tries_to_delete_team,
                Team_delete_successfully);
        }
    }
}