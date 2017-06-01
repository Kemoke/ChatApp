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
using System.Windows.Input;
using ChatApp.Model;
using System.Collections.ObjectModel;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ChatApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChatPage : Page


    {


        
        private List<UserTeam> usersTeams;
        
        public ChatPage()
        {
             
            this.InitializeComponent();

            this.ViewModel= new MessageViewModel(); 
        }
        public MessageViewModel ViewModel = new MessageViewModel(); 

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            {

                if (messageBox.Text != "")
                {

                    ViewModel.MessageList.Add(new MEssage { message8 = messageBox.Text });

                }

                messageBox.Text = ""; 


                
            }

           

            
        }

       
    }

   

    



}
