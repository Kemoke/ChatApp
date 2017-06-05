﻿using System;
using System.Collections.Generic;
using System.Text;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace ChatServerTests.Features
{
    [FeatureDescription(
        @"Every user should have role assigned to it inside of a channel.")]
    [Label("Story-7")]

    public partial class Role_Feature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Roles")]
        public void Deleting_role()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                Role_is_being_created,
                Role_is_then_deleted,
                Role_deletion_successful);
        }

        [Scenario]
        [Label("Ticket-2")]
        [ScenarioCategory("Roles")]
        public void Edit_role()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                Role_is_being_created,
                Role_is_then_edited,
                Role_edit_successful);
        }

        [Scenario]
        [Label("Ticket-3")]
        [ScenarioCategory("Roles")]
        public void Get_role()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                Role_is_being_created,
                Role_is_being_requested_by_id,
                Role_request_successful);
        }

        [Scenario]
        [Label("Ticket-4")]
        [ScenarioCategory("Roles")]
        public void Retrieve_list_of_roles()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                Given_that_several_roles_exist_in_database,
                User_requests_list_of_roles,
                Role_list_successfully_retrieved);
        }

        [Scenario]
        [Label("Ticket-5")]
        [ScenarioCategory("Roles")]
        public void Assign_role_to_user()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                Given_that_team_exists_in_database,
                Role_is_being_created,
                Role_is_assigned_to_a_user_belonging_to_a_certain_team,
                Role_assignment_successful);
        }

        [Scenario]
        [Label("Ticket-6")]
        [ScenarioCategory("Roles")]
        public void Unsign_role_to_user()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                Given_that_team_exists_in_database,
                Role_is_being_created,
                Role_is_assigned_to_a_user_belonging_to_a_certain_team,
                Role_assignment_successful,
                Users_role_is_unsigned,
                Role_unsign_successful);
        }
    }
}
