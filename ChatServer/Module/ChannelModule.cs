using Nancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer.Module
{
    public class ChannelModule : NancyModule
    {
        public ChannelModule() : base("/chat")
        {
            Get("/", _ => "This is chat module!!!!");
            //saves message
            Post("/send_message", parameters => SendMessage(parameters));
            //loads conversation
            Post("/load_messages", parameters => GetMessages(parameters));
            //checks if there are new messages
            Post("/new_messages", parametes => CheckNewMessages(parametes));
            //creates a new channel
            Post("/create_channel", parameters => CreateNewChannel(parameters));
        }

        private dynamic CreateNewChannel(dynamic parameters)
        {
            throw new NotImplementedException();
        }

        private dynamic CheckNewMessages(dynamic parametes)
        {
            throw new NotImplementedException();
        }

        private dynamic GetMessages(dynamic parameters)
        {
            throw new NotImplementedException();
        }

        private dynamic SendMessage(dynamic parameters)
        {
            throw new NotImplementedException();
        }
    }
}
