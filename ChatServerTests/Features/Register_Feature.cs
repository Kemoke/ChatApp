using System;
using System.Collections.Generic;
using System.Text;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace ChatServerTests.Features
{
    [FeatureDescription(
        @"As a user I want to be able to register")]
    [Label("Story-1")]
    public partial class Register_Feature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Security")]
        public void Successful_Registration()
        {
            Runner.RunScenario(

                Given_the_user_enters_registration_information,
                Registration_is_successful);
        }

        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Security")]
        public void Registration_attempt_when_user_already_exists()
        {
            Runner.RunScenario(
                Existing_user_in_database,
                Given_the_user_enters_registration_information,
                Registration_is_unsusccessful_because_of_existing_email);
        }
    }
}
