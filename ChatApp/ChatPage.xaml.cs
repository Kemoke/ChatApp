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
using System.Windows.Input;
using ChatApp.Model;
using System.Collections.ObjectModel;
using Windows.System;
using ChatApp.Api;
using ChatApp.Request;
using ChatApp.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ChatApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatPage : Page
    { 
       
        public ChatPage()
        {
            InitializeComponent();
        }

        private void ChatBox_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == VirtualKey.Enter)
                Button_OnClick(null, null);
        }

        private async void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = (ChatViewModel) DataContext;
            var request = new SendMessageRequest
            {
                ChannelId = viewModel.SelectedChannel.Id,
                MessageText = ChatBox.Text,
                SenderId = HttpApi.LoggedInUser.Id,
            };
            var response = await HttpApi.Channel.SendMessageAsync(request, HttpApi.AuthToken);
            viewModel.Messages.Add(response);
        }
    }
}
