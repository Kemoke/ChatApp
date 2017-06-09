using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using ChatApp.Api;
using ChatApp.Model;
using ChatApp.Request;
using Refit;

namespace ChatApp.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        private ObservableCollection<Channel> channels;
        private ObservableCollection<Message> messages;
        private Channel selectedChannel;
        private readonly Task messageLoop;

        public ObservableCollection<Channel> Channels
        {
            get => channels;
            set => SetProperty(out channels, value);
        }

        public ObservableCollection<Message> Messages
        {
            get => messages;
            set => SetProperty(out messages, value);
        }

        public Channel SelectedChannel
        {
            get => selectedChannel;
            set => SetProperty(out selectedChannel, value);
        }

        public ChatViewModel()
        {
            PropertyChanged += OnPropertyChanged;
            messageLoop = new Task(async () =>
            {
                while (true)
                {
                    await MessageLoop();
                    await Task.Delay(5000);
                }
            });
            LoadData().ConfigureAwait(false);
        }

        private async void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "SelectedChannel")
            {
                var request = new GetMessagesRequest
                {
                    ChannelId = SelectedChannel.Id
                };
                Messages = new ObservableCollection<Message>(await HttpApi.Channel.GetMessagesAsync(request, 0, 50, HttpApi.AuthToken));
                if (messageLoop.Status != TaskStatus.Running)
                    messageLoop.Start();
            }
        }

        private async Task MessageLoop()
        {
            if (selectedChannel != null)
            {
                var last = Messages.LastOrDefault();
                var lastId = last is null ? 0 : last.Id;
                var request = new CheckNewMessagesRequest
                {
                    ChannelId = SelectedChannel.Id,
                    MessageId = lastId
                };
                var response = await HttpApi.Channel.GetNewMessagesAsync(request, HttpApi.AuthToken);
                foreach (var message in response)
                {
                    Messages.Add(message);
                }
            }
        }

        private async Task LoadData()
        {
            try
            {
                var response =
                    await HttpApi.Channel.GetListAsync(new ListChannelRequest { TeamId = HttpApi.SelectedTeam.Id }, HttpApi.AuthToken);
                Channels = new ObservableCollection<Channel>(response);
            }
            catch (ApiException ex)
            {
                await new MessageDialog(ex.ErrorMessage()).ShowAsync();
            }
        }
    }
}