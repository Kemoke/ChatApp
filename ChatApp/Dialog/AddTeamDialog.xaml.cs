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
using Refit;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ChatApp.Dialog
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddTeamDialog : ContentDialog
    {
        private readonly Action<Team> callback;
        public AddTeamDialog(Action<Team> callback)
        {
            this.callback = callback;
            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var request = new CreateTeamRequest
            {
                Name = TeamNameBox.Text,
                UserId = HttpApi.LoggedInUser.Id
            };
            try
            {
                var team = await HttpApi.Team.SaveAsync(request, HttpApi.AuthToken);
                callback(team);
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
