using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ChatApp.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserInfoPage : Page
    {
        private readonly UserInfoViewModel viewModel;

        public UserInfoPage()
        {
            this.InitializeComponent();
            viewModel = (UserInfoViewModel)DataContext;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
        
        private void List_item_click(object sender, SelectionChangedEventArgs e)
        {
            gender.Text = viewModel.SelectedUser.Gender;
            country.Text = viewModel.SelectedUser.Country;
            Username.Text = viewModel.SelectedUser.Username;
            email.Text = viewModel.SelectedUser.Email;
            company.Text = viewModel.SelectedUser.Company;
            FirstName.Text = viewModel.SelectedUser.FirstName;
            LastName.Text = viewModel.SelectedUser.LastName;
            DatePicker.Text = viewModel.SelectedUser.DateOfBirth.ToString();
        }
    }
}
