using System;
using System.Collections.Generic;
using System.Text;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace ChatServerTests.Features
{
    [FeatureFixture]
    [FeatureDescription(
        @"As a user I want to be able to log in")]
    [Label("Story-2")]
    public partial class Login_Feature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Security")]
        public void Successful_login()
        {
            Runner.RunScenario(

                Given_the_user_is_already_registered,
                When_the_user_sends_login_request_with_correct_credentials,
                Then_the_login_operation_should_be_successful);
        }

        [Scenario]
        [Label("Ticket-2")]
        [ScenarioCategory("Security")]
        public void Unsuccessful_login_caused_by_wrong_username()
        {
            Runner.RunScenario(
                When_the_user_sends_login_request_with_incorrect_username,
                Then_the_login_operation_should_be_unsuccessful);
        }

        [Scenario]
        [Label("Ticket-3")]
        [ScenarioCategory("Security")]
        public void Unsuccessful_login_caused_by_wrong_password()
        {
            Runner.RunScenario(
                Given_the_user_is_already_registered,
                When_the_user_sends_login_request_with_incorrect_password,
                Then_the_login_operation_should_be_unsuccessful);
        }
    }
}
