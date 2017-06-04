using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChatApp.Model
{
    public class Team : Entity
    {
        private string name;
        private ObservableCollection<Channel> channels;
        private ObservableCollection<User> users;

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged();}
        }

        public ObservableCollection<Channel> Channels
        {
            get => channels;
            set { channels = value; OnPropertyChanged();}
        }

        public ObservableCollection<User> Users
        {
            get => users;
            set { users = value; OnPropertyChanged();}
        }
    }
}
