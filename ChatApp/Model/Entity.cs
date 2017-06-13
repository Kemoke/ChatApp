using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatApp.Model
{
    public abstract class Entity : INotifyPropertyChanged
    {
        private int id;

        public int Id {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        protected void OnPropertyChanged([CallerMemberName]string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override bool Equals(object obj)
        {
            var entity = obj as Entity;
            return id == entity?.id;
        }
    }
}
