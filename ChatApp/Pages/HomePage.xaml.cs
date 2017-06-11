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
using Windows.UI.Xaml.Navigation;
using ChatApp.Api;
using ChatApp.Dialog;
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
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
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
