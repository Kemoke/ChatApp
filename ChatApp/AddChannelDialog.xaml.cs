using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ChatApp.Api;
using ChatApp.Model;
using ChatApp.Request;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ChatApp
{
    public sealed partial class AddChannelDialog : ContentDialog
    {
        public Channel CreatedChannel;
        public AddChannelDialog(ref Channel created)
        {
            CreatedChannel = created;
            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var request = new CreateChannelRequest
            {
                ChannelName = ChannelNameBox.Text,
                TeamId = HttpApi.SelectedTeam.Id,
                UserId = HttpApi.LoggedInUser.Id
            };
            CreatedChannel = await HttpApi.Channel.SaveAsync(request, HttpApi.AuthToken);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }
    }
}
