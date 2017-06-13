using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
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
        private MessageWebSocket messageSocket;
        private int oldChannelId;
        private DataWriter writer;

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

        public Action<Message> Recieved { get; set; }

        public string TeamName => HttpApi.SelectedTeam.Name;

        public ChatViewModel()
        {
            PropertyChanged += OnPropertyChanged;
            oldChannelId = -1;
            LoadData().ConfigureAwait(false);
        }

        private async void MessageSocket_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            var msgReader = args.GetDataReader();
            var msgBytes = new byte[msgReader.UnconsumedBufferLength];
            msgReader.ReadBytes(msgBytes);
            var msg = new UTF8Encoding(false).GetString(msgBytes);
            var message = JsonConvert.DeserializeObject<Message>(msg);
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    Messages.Add(message);
                    Recieved(message);
                });
        }

        private async void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "SelectedChannel")
            {
                if (SelectedChannel == null)
                    return;
                var request = new GetMessagesRequest
                {
                    ChannelId = SelectedChannel.Id
                };
                var message = JsonConvert.SerializeObject(new NotificationMessage
                {
                    NewId = SelectedChannel.Id,
                    OldId = oldChannelId,
                    Token = HttpApi.AuthToken
                });
                writer.WriteString(message);
                await writer.StoreAsync();
                oldChannelId = SelectedChannel.Id;
                try
                {
                    var response = await HttpApi.Channel.GetMessagesAsync(request, 0, 50, HttpApi.AuthToken);
                    response.Reverse();
                    Messages = new ObservableCollection<Message>(response);
                }
                catch (ApiException ex)
                {
                    await ex.ShowErrorDialog();
                }
            }
        }

        private async Task LoadData()
        {
            messageSocket = new MessageWebSocket();
            messageSocket.Control.MessageType = SocketMessageType.Utf8;
            messageSocket.MessageReceived += MessageSocket_MessageReceived;
            await messageSocket.ConnectAsync(new Uri("ws://srv.kemoke.net:2424/notifications"));
            writer = new DataWriter(messageSocket.OutputStream);
            try
            {
                var response =
                    await HttpApi.Channel.GetListAsync(new ListChannelRequest { TeamId = HttpApi.SelectedTeam.Id }, HttpApi.AuthToken);
                Channels = new ObservableCollection<Channel>(response);
            }
            catch (ApiException ex)
            {
                await ex.ShowErrorDialog();
            }
        }
    }
}