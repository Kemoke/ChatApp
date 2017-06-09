using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatApp.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected void SetProperty<T>(out T property, T value, [CallerMemberName]string propName = null)
        {
            property = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}