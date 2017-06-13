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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ChatApp.Dialog
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddUserDialog : ContentDialog
    {

        public AddUserDialog()
        {
            this.InitializeComponent();
            Loaded += NewUserDialog_Loaded;
        }

        private async void NewUserDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var item = await HttpApi.User.GetListAsync(HttpApi.AuthToken);
            var users = item.Where(u => !HttpApi.SelectedTeam.Users.Contains(u)).ToList();
            UsernameBox.ItemsSource = users;
            RoleBox.ItemsSource = await HttpApi.Role.GetListAsync(HttpApi.AuthToken);
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var user = (User) UsernameBox.SelectionBoxItem;
            await HttpApi.Role.AssignRoleAsync(new AssignRoleRequest
            {
                RoleId = ((Role)RoleBox.SelectionBoxItem).Id,
                TeamId = HttpApi.SelectedTeam.Id,
                UserId = user.Id
            }, HttpApi.AuthToken);
            HttpApi.SelectedTeam.Users.Add(user);
            await new MessageDialog("User added successfully").ShowAsync();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Hide();
        }

    }
}
