using Nancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Module
{
    public class RoleModule : NancyModule
    {
        public RoleModule() : base("/role")
        {
            Get("/", _ => "This is role module!!!!");
            Post("/assign_role", parameters => AssignRole(parameters));
        }

        private dynamic AssignRole(object parameters)
        {
            throw new NotImplementedException();
        }
    }
}
