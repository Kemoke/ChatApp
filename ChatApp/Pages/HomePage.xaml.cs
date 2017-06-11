using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using ChatApp.ViewModel;


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
            //viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

       

        private void Team_RightClick(object sender, RightTappedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Menu_EditTeam(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Menu_DeleteTeam(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void List_item_click(object sender, SelectionChangedEventArgs e)
        {
            HttpApi.SelectedTeam = viewModel.SelectedTeam;
            Frame.Navigate(typeof(ChatPage));
        }
    }
}
