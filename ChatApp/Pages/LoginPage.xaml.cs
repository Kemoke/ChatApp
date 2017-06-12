using System;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using ChatApp.Api;
using ChatApp.Model;
using ChatApp.Request;
using Refit;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ChatApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            if (Mail.Text == "" || Password.Password == "")
            {
                var messageDialog = new MessageDialog("please do not leave blanks");
                await messageDialog.ShowAsync();
                
                return;
            }
            try
            {
                ProgressIndicator.IsActive = true;
                var response =
                    await HttpApi.Auth.LoginAsync(
                        new LoginRequest { Username = Mail.Text, Password = Password.Password });
                HttpApi.AuthToken = response.Token;
                HttpApi.LoggedInUser = response.User;
                /*HttpApi.SelectedTeam = new Team
                {
                    Name = "Team1",
                    Id = 1
                };
                Frame.Navigate(typeof(ChatPage));*/
                Frame.Navigate(typeof(HomePage));
            }
            catch (ApiException ex)
            {
                await ex.ShowErrorDialog();
                Mail.Text = "";
                Password.Password = ""; 
            }
            finally
            {
                ProgressIndicator.IsActive = false;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

            Frame.Navigate(typeof(CreateAccountPage));
        }

        private void LoginPage_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                LoginButton_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
