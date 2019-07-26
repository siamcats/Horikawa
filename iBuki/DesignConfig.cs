using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace iBuki
{
    public class DesignConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _isDateDisplay = true;
        public bool IsDateDisplay
        {
            get { return _isDateDisplay; }
            set
            {
                if (value == _isDateDisplay) return;
                _isDateDisplay = value;
                OnPropertyChanged();
            }
        }

        private string _dateDisplayFormat = "d ddd";
        public string DateDisplayFormat
        {
            get { return _dateDisplayFormat; }
            set
            {
                if (value == _dateDisplayFormat) return;
                _dateDisplayFormat = value;
                OnPropertyChanged();
            }
        }
    
    }
}
