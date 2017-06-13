using System;
using System.Collections.Generic;
using Windows.UI.Core;
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
    public sealed partial class CreateAccountPage : Page
    {
        public CreateAccountPage()
        {
            this.InitializeComponent();
            GenderBox.Items?.Add("Male");
            GenderBox.Items?.Add("Female");
            CountryBox.ItemsSource = Countries;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

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

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (MailText.Text == "" || UsernameText.Text == "" || PasswordText.Password == "" || RepeatText.Password == "")
            {
                var messageDialog = new MessageDialog("please do not leave blanks");
                await messageDialog.ShowAsync();
                return;
            }

            if (PasswordText.Password != RepeatText.Password)
            {
                var messageDialog = new MessageDialog("Passwords do not match");
                await messageDialog.ShowAsync();
                return;
            }

            if (PasswordText.Password != RepeatText.Password)
            {
                var messageDialog = new MessageDialog("Passwords do not match");
                await messageDialog.ShowAsync();
                return;
            }
            

                var user = new User
            {
                FirstName = NameText.Text,
                LastName = SurnameText.Text,
                Username = UsernameText.Text,
                Password = PasswordText.Password,
                Email = MailText.Text,
                Company = CompanyText.Text,
                DateOfBirth = DatePicker.Date.DateTime,
                Country = CountryBox.SelectedItem?.ToString(),
                Gender = GenderBox.SelectedItem?.ToString()
            };

            try
            {
                await HttpApi.Auth.RegisterAsync(new RegisterRequest { User = user });
                Frame.Navigate(typeof(LoginPage));
            }
            catch (ApiException ex)
            {
                await ex.ShowErrorDialog();
            }
        }
    }
}
