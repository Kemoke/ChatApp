using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace ChatServerTests.Features
{
    [FeatureFixture]
    [FeatureDescription(
        @"As a user I want to be able to log in")]
    [Label("Story-2")]
    public partial class LoginFeature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory(Category.Security)]
        public Task Successful_login()
        {
            return Runner.RunScenarioAsync(

                Given_the_user_is_already_registered,
                When_the_user_sends_login_request_with_correct_credentials,
                Then_the_login_operation_should_be_successful);
        }

        [Scenario]
        [Label("Ticket-2")]
        [ScenarioCategory(Category.Security)]
        public Task Unsuccessful_login_caused_by_wrong_username()
        {
            return Runner.RunScenarioAsync(
                When_the_user_sends_login_request_with_incorrect_username,
                Then_the_login_operation_should_be_unsuccessful);
        }

        [Scenario]
        [Label("Ticket-3")]
        [ScenarioCategory(Category.Security)]
        public Task Unsuccessful_login_caused_by_wrong_password()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_already_registered,
                When_the_user_sends_login_request_with_incorrect_password,
                Then_the_login_operation_should_be_unsuccessful);
        }
    }
}
