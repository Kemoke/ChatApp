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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ChatApp.Dialog
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewUserDialog : ContentDialog
    {
        private User user;
        public NewUserDialog(User user, List<Role> roles)
        {
            this.user = user;
            this.InitializeComponent();
            UsernameBox.Text = user.Username;
            RoleBox.ItemsSource = roles;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            await HttpApi.Role.AssignRoleAsync(new AssignRoleRequest
            {
                RoleId = ((Role)RoleBox.SelectionBoxItem).Id,
                TeamId = HttpApi.SelectedTeam.Id,
                UserId = user.Id
            }, HttpApi.AuthToken);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

    }
}
