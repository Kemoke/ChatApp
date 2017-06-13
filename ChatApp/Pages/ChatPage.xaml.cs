using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using ChatApp.Api;
using ChatApp.Dialog;
using ChatApp.Model;
using ChatApp.Request;
using ChatApp.ViewModel;
using Refit;
using User = ChatApp.Model.User;

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
            viewModel.Recieved = async message =>
            {
                await Task.Delay(100);
                MessageView.ScrollIntoView(viewModel.Messages.LastOrDefault());
            };
            MessageView.Loaded += (s, a) =>
            {
                var scrollViewer = GetScrollViewer(MessageView);
                scrollViewer.ViewChanged += ScrollViewer_ViewChanged;
            };
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer) sender;
            if (scrollViewer.VerticalOffset < 0.001)
            {
                LoadingRing.IsActive = true;
                LoadingRing.Visibility = Visibility.Visible;
                var skip = viewModel.Messages.Count;
                var item = viewModel.Messages.FirstOrDefault();
                try
                {
                    var request = new GetMessagesRequest
                    {
                        ChannelId = viewModel.SelectedChannel.Id
                    };
                    var result = await HttpApi.Channel.GetMessagesAsync(request, skip, 50, HttpApi.AuthToken);
                    for (var i = result.Count - 1; i >= 0; i--)
                    {
                        viewModel.Messages.Insert(0, result[i]);
                    }
                    MessageView.ScrollIntoView(item);
                }
                catch (ApiException ex)
                {
                    await ex.ShowErrorDialog();
                }
                finally
                {
                    LoadingRing.IsActive = false;
                    LoadingRing.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Messages")
            {
                await Task.Delay(100);
                MessageView.ScrollIntoView(viewModel.Messages.LastOrDefault());
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
            try
            {
                await new EditChannelDialog(item, channel =>
                {
                    var index = viewModel.Channels.IndexOf(item);
                    if (viewModel.SelectedChannel.Equals(viewModel.Channels[index]))
                    {
                        viewModel.Channels[index] = channel;
                        viewModel.SelectedChannel = viewModel.Channels[index];
                    }
                    else
                    {
                        viewModel.Channels[index] = channel;
                    }
                }).ShowAsync();
            }
            catch
            {
                //
            }
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

        private static ScrollViewer GetScrollViewer(DependencyObject o)
        {
            // Return the DependencyObject if it is a ScrollViewer
            if (o is ScrollViewer viewer)
            {
                return viewer;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {
                var child = VisualTreeHelper.GetChild(o, i);

                var result = GetScrollViewer(child);
                if (result == null)
                {
                    continue;
                }
                return result;
            }
            return null;
        }

        private void MessageView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void UserListButton_Click(object sender, RoutedEventArgs e)
        {
            var item = HttpApi.SelectedTeam.Users.ToList();

            await new UserListDialog(item).ShowAsync();
        }

        private async void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            await new AddUserDialog().ShowAsync();
        }

        private async void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            await new RemoveUserDialog().ShowAsync();
        }
    }
}
