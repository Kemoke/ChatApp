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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ChatApp.Dialog
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddUserDialog : ContentDialog
    {
        private readonly Action<User> callback;

        public AddUserDialog(List<User> items)
        {
            this.InitializeComponent();
            UserList.ItemsSource = items;
        }
        
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

        private async void MySelection(object sender, SelectionChangedEventArgs e)
        {
                var roles = await HttpApi.Role.GetListAsync(HttpApi.AuthToken);
                var user = (User) UserList.SelectedItem;
                await new NewUserDialog(user, roles).ShowAsync();
        }
    }
}
