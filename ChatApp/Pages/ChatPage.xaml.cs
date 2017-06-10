using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using ChatApp.Api;
using ChatApp.Dialog;
using ChatApp.Model;
using ChatApp.Request;
using ChatApp.ViewModel;
using Refit;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ChatApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatPage : Page
    {
        private readonly ChatViewModel viewModel;
        public ChatPage()
        {
            InitializeComponent();
            viewModel = (ChatViewModel)DataContext;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private async void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Messages")
            {
                await Task.Delay(100);
                MessageView.ScrollIntoView(viewModel.Messages.LastOrDefault());
                viewModel.Messages.CollectionChanged += async (o, args) =>
                {
                    if (args.Action == NotifyCollectionChangedAction.Add)
                    {
                        await Task.Delay(100);
                        MessageView.ScrollIntoView(viewModel.Messages.LastOrDefault());
                    }
                };
            }
        }

        private void ChatBox_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                Button_OnClick(sender, e);
                e.Handled = true;
            }
        }

        private async void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var request = new SendMessageRequest
            {
                ChannelId = viewModel.SelectedChannel.Id,
                MessageText = ChatBox.Text,
                SenderId = HttpApi.LoggedInUser.Id,
                TargetId = HttpApi.LoggedInUser.Id
            };
            ChatBox.Text = "";
            try
            {
                var response = await HttpApi.Channel.SendMessageAsync(request, HttpApi.AuthToken);
            }
            catch (ApiException ex)
            {
                await ex.ShowErrorDialog();
            }
        }

        private async void NewChannelButton_Click(object sender, RoutedEventArgs e)
        {
            await new AddChannelDialog(channel => viewModel.Channels.Add(channel)).ShowAsync();
        }

        private void Channel_RightClick(object sender, RightTappedRoutedEventArgs e)
        {
            var item = (Grid)sender;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(item);
            flyoutBase.ShowAt(item);
        }

        private async void Menu_EditChannel(object sender, RoutedEventArgs e)
        {
            var item = (Channel)((FrameworkElement)e.OriginalSource).DataContext;
            await new EditChannelDialog(item, channel =>
            {
                var index = viewModel.Channels.IndexOf(item);
                viewModel.Channels[index] = channel;
            }).ShowAsync();
        }

        private async void Menu_DeleteChannel(object sender, RoutedEventArgs e)
        {
            var item = (Channel)((FrameworkElement)e.OriginalSource).DataContext;
            try
            {
                await HttpApi.Channel.DeleteAsync(item.Id, HttpApi.AuthToken);
                viewModel.Channels.Remove(item);
            }
            catch (ApiException ex)
            {
                await ex.ShowErrorDialog();
            }
        }
    }
}
