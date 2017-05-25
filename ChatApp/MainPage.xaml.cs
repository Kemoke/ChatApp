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
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.Web.Http;
using ChatApp.Model;
using ChatApp.Response;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ChatApp

{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {

            if (mail.Text == "" || password.Password == "")
            {
                var messageDialog = new MessageDialog("please do not leave blanks");
                await messageDialog.ShowAsync();
                return;
            }
            var client = new HttpClient();
            var response = await client.PostAsync("http://localhost:1337/auth/login", 
                new User { Username = mail.Text, Password = password.Password });
            if (response.StatusCode != HttpStatusCode.Ok)
            {
                var msg = await response.ErrorMessage();
                var messageDialog = new MessageDialog(msg);
                await messageDialog.ShowAsync();
                return;
            }
            var user = await response.JsonBody<UserInfo>();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
           Frame.Navigate(typeof(CreateAccount));
        }


    }
}
