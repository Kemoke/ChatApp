using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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

namespace ChatApp.Dialog
{
    public sealed partial class RemoveUserDialog : ContentDialog
    {
        public RemoveUserDialog()
        {
            this.InitializeComponent();
            UsernameBox.ItemsSource = HttpApi.SelectedTeam.Users.ToList();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var user = (User) UsernameBox.SelectionBoxItem;
            await HttpApi.Role.UnAssignRoleAsync(new UnsignRoleRequest
            {
                TeamId = HttpApi.SelectedTeam.Id,
                UserId = user.Id
            }, HttpApi.AuthToken);
            HttpApi.SelectedTeam.Users.Remove(user);
            await new MessageDialog("User removed successfully").ShowAsync();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

    }
}
