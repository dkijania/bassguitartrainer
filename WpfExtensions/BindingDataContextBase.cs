using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfExtensions
{
    public abstract class BindingDataContextBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(
                [CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
