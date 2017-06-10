﻿using System;
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
using Refit;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ChatApp.Dialog
{
    public sealed partial class AddChannelDialog : ContentDialog
    {
        private readonly Action<Channel> callback;
        public AddChannelDialog(Action<Channel> callback)
        {
            this.callback = callback;
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
            try
            {
                var channel = await HttpApi.Channel.SaveAsync(request, HttpApi.AuthToken);
                callback(channel);
            }
            catch (ApiException ex)
            {
                await ex.ShowErrorDialog();
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }
    }
}
