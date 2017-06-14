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
        @"Every user should have role assigned to it inside of a channel.")]
    [Label("Story-7")]

    public partial class RoleFeature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory(Category.Role)]
        public Task Deleting_role()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Role_is_being_created,
                Role_is_then_deleted,
                Role_deletion_successful);
        }

        [Scenario]
        [Label("Ticket-2")]
        [ScenarioCategory(Category.Role)]
        public Task Edit_role()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Role_is_being_created,
                Role_is_then_edited,
                Role_edit_successful);
        }

        [Scenario]
        [Label("Ticket-3")]
        [ScenarioCategory(Category.Role)]
        public Task Get_role()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Role_is_being_created,
                Role_is_being_requested_by_id,
                Role_request_successful);
        }

        [Scenario]
        [Label("Ticket-4")]
        [ScenarioCategory(Category.Role)]
        public Task Retrieve_list_of_roles()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_that_several_roles_exist_in_database,
                User_requests_list_of_roles,
                Role_list_successfully_retrieved);
        }

        [Scenario]
        [Label("Ticket-5")]
        [ScenarioCategory(Category.Role)]
        public Task Assign_role_to_user()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_that_team_exists_in_database,
                Role_is_being_created,
                Role_is_assigned_to_a_user_belonging_to_a_certain_team,
                Role_assignment_successful);
        }

        [Scenario]
        [Label("Ticket-6")]
        [ScenarioCategory(Category.Role)]
        public Task Unsign_role_to_user()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                Given_that_team_exists_in_database,
                Role_is_being_created,
                Role_is_assigned_to_a_user_belonging_to_a_certain_team,
                Role_assignment_successful,
                Users_role_is_unassigned,
                Role_unassign_successful);
        }
    }
}
