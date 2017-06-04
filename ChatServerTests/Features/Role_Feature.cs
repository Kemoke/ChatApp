using System;
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
        public void Successfuly_create_channel()
        {
            Runner.RunScenario(
                Given_the_user_is_logged_in,
                Given_the_user_creates_team_and_is_admin,
                User_tries_to_create_new_channel_providing_channel_name,
                Channel_creation_successful);
        }
    }
}
