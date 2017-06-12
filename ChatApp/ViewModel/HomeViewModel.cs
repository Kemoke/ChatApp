using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using ChatApp.Api;
using ChatApp.Model;
using ChatApp.Request;
using Coding4Fun.Toolkit.Controls;
using Newtonsoft.Json;
using Refit;

namespace ChatApp.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private ObservableCollection<Team> teams;
        private Team selectedTeam;
        private DispatcherTimer timer;
        private DataWriter writer;

        public HomeViewModel()
        {
            //PropertyChanged += OnPropertyChanged;
            timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 5) };
            timer.Tick += async (sender, o) =>
            {
                var response = await HttpApi.Team.GetListAsync(HttpApi.AuthToken);
                foreach (var team in response)
                {
                    Teams.Add(team);
                }
            };
            
            LoadData().ConfigureAwait(false);
        }

        public ObservableCollection<Team> Teams
        {
            get => teams;
            set => SetProperty(out teams, value);
        }

        public Team SelectedTeam
        {
            get => selectedTeam;
            set => SetProperty(out selectedTeam, value);
        }

        public User LoggedInUser => HttpApi.LoggedInUser;

        private async Task LoadData()
        {
            try
            {
                var response =
                    await HttpApi.Team.GetListAsync(HttpApi.AuthToken);
                Teams = new ObservableCollection<Team>(response);
            }
            catch (ApiException ex)
            {
                await ex.ShowErrorDialog();
            }
        }

        private async void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            
        }

    }
}