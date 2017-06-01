using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace ChatApp
{
    public class MEssage
    {
        public string message8 { get; set; }

        public MEssage()
        {
            this.message8 = "message1";
        }
    }

    public class MessageViewModel
    {


        private MEssage defaultMessage = new MEssage();
        public MEssage DefaultMessage { get { return this.defaultMessage; } }
        private ObservableCollection<MEssage> messageList = new ObservableCollection<MEssage>();
        public ObservableCollection<MEssage> MessageList { get { return this.messageList; } }

        public MessageViewModel()
        {
            this.MessageList.Add(new MEssage() { message8 = ""});
        }




    }

}
