using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {

            if (Mail.Text == "" || Password.Password == "")
            {
                var messageDialog = new MessageDialog("please do not leave blanks");
                await messageDialog.ShowAsync();
                return;
            }
            try
            {
                var response = await HttpApi.Auth.LoginAsync(new LoginRequest { Username = Mail.Text, Password = Password.Password });
                HttpApi.AuthToken = response.Token;
                HttpApi.LoggedInUser = response.User;
                HttpApi.SelectedTeam = new Team
                {
                    Name = "Team1",
                    Id = 1
                };
                Frame.Navigate(typeof(ChatPage));
            }
            catch (ApiException ex)
            {
                await ex.ShowErrorDialog();
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

            Frame.Navigate(typeof(CreateAccountPage));
        }
    }
}
