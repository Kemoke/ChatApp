using System;
using System.Collections.Generic;
using System.Text;
using ChatServer.Model;
using LightBDD.XUnit2;
using Xunit.Abstractions;

namespace ChatServerTests.Features
{
    public partial class Role_Feature : FeatureFixture
    {
        #region Setup/Teardown

        private readonly FeaturesConfig config;
        private readonly User user;


        public Role_Feature(ITestOutputHelper output) : base(output)
        {

            config = new FeaturesConfig();

            user = DataGenerator.GenerateSingleUser(config.Context);

            
        }

        #endregion



    }
}
