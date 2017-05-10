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

        String mail1 = "din997@gmail.com";

        String password1 = "martinmartin";

        





        private async void button1_Click(object sender, RoutedEventArgs e)
        {

            if (mail.Text == "" || password.Password == "")
            {
                var MessageDialog = new MessageDialog("please do not leave blanks");

                await MessageDialog.ShowAsync();


            }



            else if (mail1 != mail.Text && password1 != password.Password)
            {
                var messageDialog = new MessageDialog("Password or mail are incorrect");

                mail.Text = "";
                password.Password = "";

                await messageDialog.ShowAsync();
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {


           Frame.Navigate(typeof(CreateAccount));
        }


    }
}
