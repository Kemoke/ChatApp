using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using ChatApp.Api;
using ChatApp.Model;
using ChatApp.Request;
using Newtonsoft.Json;
using Refit;

namespace ChatApp.ViewModel
{
    public class ChatViewModel : ViewModelBase
    {
        private ObservableCollection<Channel> channels;
        private ObservableCollection<Message> messages;
        private Channel selectedChannel;
        private DispatcherTimer timer;
        private MessageWebSocket messageSocket;
        private int oldChannelId;

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
            timer = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 5)};
            timer.Tick += async (sender, o) =>
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
            };
            oldChannelId = -1;
            LoadData().ConfigureAwait(false);
        }

        private void MessageSocket_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            var msgReader = args.GetDataReader();
            msgReader.UnicodeEncoding = UnicodeEncoding.Utf8;
            var msg = msgReader.ReadString(msgReader.UnconsumedBufferLength);
            var message = JsonConvert.DeserializeObject<Message>(msg);
            Messages.Add(message);
        }

        private async void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "SelectedChannel")
            {
                var request = new GetMessagesRequest
                {
                    ChannelId = SelectedChannel.Id
                };
                //timer.Stop();
                using (var writer = new DataWriter(messageSocket.OutputStream))
                {
                    var message = JsonConvert.SerializeObject(new NotificationMessage
                    {
                        NewId = SelectedChannel.Id,
                        OldId = oldChannelId,
                        Token = HttpApi.AuthToken
                    });
                    writer.WriteString(message);
                }
                Messages = new ObservableCollection<Message>(await HttpApi.Channel.GetMessagesAsync(request, 0, 50, HttpApi.AuthToken));
                //timer.Start();
            }
        }

        private async Task LoadData()
        {
            messageSocket = new MessageWebSocket();
            messageSocket.Control.MessageType = SocketMessageType.Utf8;
            messageSocket.MessageReceived += MessageSocket_MessageReceived;
            await messageSocket.ConnectAsync(new Uri(HttpApi.ApiUrl));
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