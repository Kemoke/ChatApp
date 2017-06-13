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
        private static readonly List<string> Countries = new List<string>
        {
            "Afghanistan","Albania","Algeria", "American Samoa","Andorra","Angola","Anguilla",
            "Antarctica","Antigua and Barbuda", "Argentina","Armenia", "Aruba","Australia","Austria","Azerbaijan","Bahamas", "Bahrain","Bangladesh", "Barbados",
            "Belarus","Belgium","Belize","Benin","Bermuda","Bhutan","Bolivia","Bosnia and Herzegovina", "Botswana", "Bouvet Island",
            "Brazil", "British Indian Ocean Territory", "Brunei Darussalam","Bulgaria","Burkina Faso", "Burundi",  "Cambodia", "Cameroon","Canada","Cape Verde","Cayman Islands",
            "Central African Republic","Chad","Chile","China", "Christmas Island","Cocos (Keeling) Islands","Colombia","Comor","Congo","Congo, the Democratic Republic of the","Cook Islands", "Costa Rica",
            "Cote D'Ivoire","Croatia", "Cuba","Cyprus","Czech Republic", "Denmark", "Djibouti", "Dominica","Dominican Republic","Ecuador",
            "Egypt","El Salvador","Equatorial Guinea","Eritrea", "Estonia","Ethiopia","Falkland Islands (Malvinas)","Faroe Islands",
            "Fiji","Finland", "France","French Guiana","French Polynesia","French Southern Territories","Gabon","Gambia","Georgia","Germany","Ghana","Gibraltar","Greece",
            "Greenland","Grenada","Guadeloupe","Guam","Guatemala","Guinea","Guinea-Bissau","Guyana","Haiti","Heard Island and Mcdonald Islands",
            "Holy See (Vatican City State)","Honduras","Hong Kong","Hungary","Iceland","India","Indonesia","Iran, Islamic Republic of","Iraq",
            "Ireland","Israel","Italy","Jamaica","Japan","Jordan","Kazakhstan","Kenya","Kiribati","Korea, Democratic People's Republic of",
            "Korea, Republic of","Kuwait","Kyrgyzstan","Lao People's Democratic Republic","Latvia","Lebanon","Lesotho","Liberia","Libya","Liechtenstein","Lithuania","Luxembourg","Macao", "Macedonia, the Former Yugoslav Republic of",
            "Madagascar","Malawi","Malaysia","Maldives","Mali", "Malta","Marshall Islands","Martinique", "Mauritania","Mauritius",
            "Mayotte","Mexico","Micronesia, Federated States of","Moldova, Republic of", "Monaco", "Mongolia","Montserrat",
            "Morocco","Mozambique","Myanmar","Namibia","Nauru","Nepal","Netherlands","Netherlands Antilles","New Caledonia","New Zealand","Nicaragua","Niger",
            "Nigeria","Niue","Norfolk Island","Northern Mariana Islands","Norway","Oman","Pakistan","Palau","Palestinian Territory, Occupied","Panama",
            "Papua New Guinea","Paraguay","Peru","Philippines","Pitcairn","Poland","Portugal","Puerto Rico",
            "Qatar","Reunion","Romania","Russian Federation","Rwanda","Saint Helena","Saint Kitts and Nevis","Saint Lucia","Saint Pierre and Miquelo", "Saint Vincent and the Grenadines",
            "Samoa", "San Marino", "Sao Tome and Principe","Saudi Arabia",
            "Senegal","Serbia","Montenegro","Seychelles","Sierra Leone","Singapore","Slovakia","Slovenia","Solomon Islands", "Somalia","South Africa","South Georgia and the South Sandwich Islands","Spain","Sri Lanka","Sudan","Suriname","Svalbard and Jan Mayen", "Swaziland","Sweden",
            "Switzerland","Syrian Arab Republic","Taiwan, Province of China","Tajikistan","Tanzania, United Republic of", "Thailand",
            "Timor-Leste","Togo","Tokelau","Tonga","Trinidad and Tobago","Tunisia","Turkey","Turkmenistan","Turks and Caicos Islands","Tuvalu","Uganda",
            "Ukraine","United Arab Emirates","United Kingdom","United States","United States Minor Outlying Islands","Uruguay","Uzbekistan",
            "Vanuatu","Venezuela","Vietnam","Virgin Islands, British","Virgin Islands, US","Wallis and Futuna","Western Sahara", "Yemen","Zambia",
            "Zimbabwe",
        };

        private readonly HomeViewModel viewModel;

        public HomePage()
        {
            this.InitializeComponent();
            gender.Items.Add("Female");
            gender.Items.Add("Male");
            country.ItemsSource = Countries; 
            viewModel = (HomeViewModel)DataContext;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            InitializeLoggedinUserInfo();
        }


        private void InitializeLoggedinUserInfo()
        {
            Username.Text = HttpApi.LoggedInUser.Username;
            email.Text = HttpApi.LoggedInUser.Email;
            company.Text = HttpApi.LoggedInUser.Company;
            FirstName.Text = HttpApi.LoggedInUser.FirstName;
            LastName.Text = HttpApi.LoggedInUser.LastName;
            country.SelectedItem = HttpApi.LoggedInUser.Country;
            DatePicker.Date = HttpApi.LoggedInUser.DateOfBirth;
            gender.SelectedItem = HttpApi.LoggedInUser.Gender;
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
                        Country = country.SelectedItem.ToString(),
                        DateOfBirth = DatePicker.Date.DateTime,
                        Gender = gender.SelectedItem.ToString()
                    }, HttpApi.AuthToken);

                    HttpApi.LoggedInUser.Username = Username.Text;

                    HttpApi.LoggedInUser.Company = company.Text;
                    HttpApi.LoggedInUser.FirstName = FirstName.Text;
                    HttpApi.LoggedInUser.LastName = LastName.Text;
                    HttpApi.LoggedInUser.Country = country.SelectedItem.ToString();
                    HttpApi.LoggedInUser.DateOfBirth = DatePicker.Date.DateTime;
                    HttpApi.LoggedInUser.Gender = gender.SelectedItem.ToString();

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

        private void UserListButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserInfoPage));
        }
    }
}
