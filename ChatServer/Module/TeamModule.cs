using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Module
{
    class TeamModule : SecureModule
    {
        public TeamModule(GlobalConfig config) : base("team", config)
        {
        }
    }
}
