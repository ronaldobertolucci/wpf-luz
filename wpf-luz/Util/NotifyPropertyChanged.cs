using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_luz.Util
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void Notify(object obj, string propertyName)
        {
            PropertyChanged?.Invoke(obj, new PropertyChangedEventArgs(propertyName));
        }
    }
}
