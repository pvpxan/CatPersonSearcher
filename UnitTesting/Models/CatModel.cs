using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace UnitTesting
{
    public class CatModel : ViewModelBase
    {
        // -----------------------------------------------------
        private BitmapImage _Portrait = null;
        public BitmapImage Portrait
        {
            get { return _Portrait; }
            set
            {
                _Portrait = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Portrait"));
            }
        }

        // -----------------------------------------------------
        private string _Filename = null;
        public string Filename
        {
            get { return _Filename; }
            set
            {
                _Filename = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Filename"));
            }
        }
    }
}
