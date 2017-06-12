using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ChatApp.Api;
using ChatApp.Dialog;
using ChatApp.Model;
using ChatApp.Request;
using ChatApp.ViewModel;
using Refit;
using User = Windows.System.User;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ChatApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private readonly HomeViewModel viewModel;

        public HomePage()
        {
            this.InitializeComponent();
            viewModel = (HomeViewModel)DataContext;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            UserImage.Source =
                new BitmapImage(new Uri("https://scontent.flju2-1.fna.fbcdn.net/v/t1.0-9/11391338_1064066726954436_6934638157053324450_n.jpg?oh=fb94393d33f67b53c582670081a956c4&oe=59A401EC", UriKind.RelativeOrAbsolute));
            Username.Text = HttpApi.LoggedInUser.Username;
            email.Text = HttpApi.LoggedInUser.Email;
            company.Text = HttpApi.LoggedInUser.Company;
            FirstName.Text = HttpApi.LoggedInUser.FirstName;
            LastName.Text = HttpApi.LoggedInUser.LastName;
            country.Text = HttpApi.LoggedInUser.Country;
            DatePicker.Date = HttpApi.LoggedInUser.DateOfBirth;
            gender.Text = HttpApi.LoggedInUser.Gender;
        }



        private void Team_RightClick(object sender, RightTappedRoutedEventArgs e)
        {
            var item = (Grid)sender;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(item);
            flyoutBase.ShowAt(item);
        }
        

        private async void Menu_EditTeam(object sender, RoutedEventArgs e)
        {
            var item = (Team)((FrameworkElement)e.OriginalSource).DataContext;

            await new EditTeamDialog(item, team =>
            {
                var index = viewModel.Teams.IndexOf(item);
                viewModel.Teams[index] = team;
            }).ShowAsync();
        }

        private async void Menu_DeleteTeam(object sender, RoutedEventArgs e)
        {
            var item = (Team)((FrameworkElement)e.OriginalSource).DataContext;
            try
            {
                await HttpApi.Team.DeleteAsync(item.Id, HttpApi.AuthToken);
                viewModel.Teams.Remove(item);
            }
            catch (ApiException ex)
            {
                await ex.ShowErrorDialog();
            }
        }

        private void List_item_click(object sender, SelectionChangedEventArgs e)
        {
            HttpApi.SelectedTeam = viewModel.SelectedTeam;
            Frame.Navigate(typeof(ChatPage));
        }

        private async void NewTeamButton_Click(object sender, RoutedEventArgs e)
        {
            await new AddTeamDialog(team => viewModel.Teams.Add(team)).ShowAsync();
        }


        private async void Edit_info(object sender, RoutedEventArgs e)
        {
            if ((string)Edit_UserInfo.Content == "Edit Info")
            {
                Username.IsEnabled = true;
                company.IsEnabled = true;
                FirstName.IsEnabled = true;
                LastName.IsEnabled = true;
                country.IsEnabled = true;
                DatePicker.IsEnabled = true;
                gender.IsEnabled = true;

                Edit_UserInfo.Content = "Edit";
            }
            else
            {
                try
                {
                    var response = await HttpApi.User.EditInfoAsync(new EditUserInfoRequest()
                    {
                        Username = Username.Text,

                        Company = company.Text,
                        FirstName = FirstName.Text,
                        LastName = LastName.Text,
                        Country = country.Text,
                        DateOfBirth = DatePicker.Date.DateTime,
                        Gender = gender.Text
                    }, HttpApi.AuthToken);

                    HttpApi.LoggedInUser.Username = Username.Text;

                    HttpApi.LoggedInUser.Company = company.Text;
                    HttpApi.LoggedInUser.FirstName = FirstName.Text;
                    HttpApi.LoggedInUser.LastName = LastName.Text;
                    HttpApi.LoggedInUser.Country = country.Text;
                    HttpApi.LoggedInUser.DateOfBirth = DatePicker.Date.DateTime;
                    HttpApi.LoggedInUser.Gender = gender.Text;

                    Username.IsEnabled = false;
                    email.IsEnabled = false;
                    company.IsEnabled = false;
                    FirstName.IsEnabled = false;
                    LastName.IsEnabled = false;
                    country.IsEnabled = false;
                    DatePicker.IsEnabled = false;
                    gender.IsEnabled = false;

                    Edit_UserInfo.Content = "Edit Info";
                }
                catch (Exception ex)
                {
                    
                }
               
            }
        }
    }
}
