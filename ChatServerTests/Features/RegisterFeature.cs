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
        @"As a user I want to be able to register")]
    [Label("Story-1")]
    public partial class RegisterFeature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory(Category.Security)]
        public Task Successful_Registration()
        {
            return Runner.RunScenarioAsync(

                Given_the_user_enters_registration_information,
                Registration_is_successful);
        }

        [Scenario]
        [Label("Ticket-2")]
        [ScenarioCategory(Category.Security)]
        public Task Registration_attempt_when_user_already_exists()
        {
            return Runner.RunScenarioAsync(
                Existing_user_in_database,
                Given_the_user_enters_registration_information,
                Registration_is_unsusccessful_because_of_existing_email);
        }
    }
}
