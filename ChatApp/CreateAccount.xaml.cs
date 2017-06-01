using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Web.Http;
using ChatApp.Model;
using ChatApp.Response;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ChatApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateAccount : Page
    {
        public CreateAccount()
        {
            this.InitializeComponent();

        }



        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void createButton_Click(object sender, RoutedEventArgs e)
        {
            if (mailText.Text == "" || usernameText.Text == "" || passwordText.Password == "" || repeatText.Password == "")
            {
                var messageDialog = new MessageDialog("please do not leave blanks");
                await messageDialog.ShowAsync();
                return;
            }

            else if(passwordText.Password != repeatText.Password)
            {
                var messageDialog = new MessageDialog("Passwords do not match");
                await messageDialog.ShowAsync();
                return;
            }

            else
            {

            }
        }
    }
}
