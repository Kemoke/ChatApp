using System;
using Windows.UI.Xaml;
using ChatApp.Api;

namespace ChatApp.Model
{
    public class Message : Entity
    {
        private string messageText;
        private User sender;
        private User target;
        private Channel channel;
        private DateTime timeSent;

        public string MessageText
        {
            get => messageText;
            set { messageText = value; OnPropertyChanged();}
        }

        public User Sender
        {
            get => sender;
            set { sender = value; OnPropertyChanged();}
        }

        public User Target
        {
            get => target;
            set { target = value; OnPropertyChanged();}
        }

        public Channel Channel
        {
            get => channel;
            set { channel = value; OnPropertyChanged();}
        }

        public DateTime TimeSent
        {
            get => timeSent;
            set { timeSent = value; OnPropertyChanged();}
        }

        public string Time => TimeSent.ToString("t");
    }

}
