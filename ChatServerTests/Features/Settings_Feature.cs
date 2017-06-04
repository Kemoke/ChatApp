﻿using System;
using System.Collections.Generic;
using System.Text;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace ChatServerTests.Features
{
    [FeatureDescription(
        @"As an user I want to be able to change my personal information.")]
    [Label("Story-6")]

    public partial class Settings_Feature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Settings")]
        public void Personal_info_chage()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                User_wants_to_change_his_personal_info,
                Info_change_successful);
        }


        [Scenario]
        [Label("Ticket-2")]
        [ScenarioCategory("Settings")]
        public void Password_change()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                User_wants_to_change_his_password,
                Password_change_successful);
        }

        [Scenario]
        [Label("Ticket-3")]
        [ScenarioCategory("Settings")]
        public void Password_change_attempt_with_wrong_old_password()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                User_wants_to_change_his_password_and_has_provided_wrong_old_password,
                Password_change_unsuccessful);
        }

        [Scenario]
        [Label("Ticket-4")]
        [ScenarioCategory("Settings")]
        public void Retriving_information_about_logged_in_user()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                User_wants_to_retrieve_info_about_himself,
                Info_retrieval_successful);
        }
    }
}
