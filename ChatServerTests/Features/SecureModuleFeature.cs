using System.Threading.Tasks;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace ChatServerTests.Features
{
    [FeatureDescription(
        @"As a user I care about my privacy. I need protection to prevent malicious actions.")]
    [Label("Story-8")]

    public partial class SecureModuleFeature
    {
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory("Channel")]
        public Task User_tries_to_perform_request_without_authentication_header()
        {
            return Runner.RunScenarioAsync(
                Given_the_user_is_logged_in,
                User_tries_to_perform_request_with_wrong_token,
                Request_failed);
        }
    }
}