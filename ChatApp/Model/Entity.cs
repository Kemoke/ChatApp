using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ChatApp.Model
{
    public abstract class Entity : INotifyPropertyChanged
    {
        private int id;
        [Key]
        public int Id {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        protected void OnPropertyChanged([CallerMemberName]string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
