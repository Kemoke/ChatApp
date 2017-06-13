using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using ChatApp.Api;
using ChatApp.Model;
using Refit;

namespace ChatApp.ViewModel
{
    public class UserInfoViewModel : ViewModelBase
    {
        private ObservableCollection<User> users;
        private User selectedUser;
        private DispatcherTimer timer;
        private DataWriter writer;

        public UserInfoViewModel()
        {
            //PropertyChanged += OnPropertyChanged;
            timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 5) };
            timer.Tick += async (sender, o) =>
            {
                var response = await HttpApi.User.GetListAsync(HttpApi.AuthToken);
                foreach (var user in response)
                {
                    Users.Add(user);
                }
            };

            LoadData().ConfigureAwait(false);
        }

        public ObservableCollection<User> Users
        {
            get => users;
            set => SetProperty(out users, value);
        }

        public User SelectedUser
        {
            get => selectedUser;
            set => SetProperty(out selectedUser, value);
        }

        private async Task LoadData()
        {
            try
            {
                var response =
                    await HttpApi.User.GetListAsync(HttpApi.AuthToken);
                Users = new ObservableCollection<User>(response);
            }
            catch (ApiException ex)
            {
                await ex.ShowErrorDialog();
            }
        }
    }
}
