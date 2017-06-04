using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChatApp.Model
{
    public class Channel : Entity
    {
        private string channelName;
        private Team team;
        private ObservableCollection<Message> messages;

        public string ChannelName
        {
            get => channelName;
            set { channelName = value; OnPropertyChanged();}
        }

        public Team Team
        {
            get => team;
            set { team = value; OnPropertyChanged();}
        }

        public ObservableCollection<Message> Messages
        {
            get => messages;
            set { messages = value; OnPropertyChanged();}
        }
    }
}
