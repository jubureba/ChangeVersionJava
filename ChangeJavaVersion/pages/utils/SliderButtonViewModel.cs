using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.pages.utils {
    internal class SliderButtonViewModel : System.ComponentModel.INotifyPropertyChanged {
        private bool _enabled = false;
        public bool IsEnabled {
            get {
                return _enabled;
            }
            set {
                _enabled = value;
                NotifyPropertyChanged("IsEnabled");
            }
        }

        #region System.ComponentModel.INotifyPropertyChanged

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion System.ComponentModel.INotifyPropertyChanged
    }
}
