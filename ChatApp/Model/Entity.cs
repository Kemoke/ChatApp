using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Model
{
    public abstract class Entity : INotifyPropertyChanged
    {
        private int id;
        [Key]
        public int Id { get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
            }

        private void OnPropertyChanged(string v)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(v));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
