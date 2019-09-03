using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    public class AllTablesModel : ViewModelBase
    {
        // -----------------------------------------------------
        private string _Firstname = "";
        public string Firstname
        {
            get { return _Firstname; }
            set
            {
                _Firstname = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Firstname"));
            }
        }

        // -----------------------------------------------------
        private string _Lastname = "";
        public string Lastname
        {
            get { return _Lastname; }
            set
            {
                _Lastname = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Lastname"));
            }
        }

        // -----------------------------------------------------
        private string _Street = "";
        public string Street
        {
            get { return _Street; }
            set
            {
                _Street = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Street"));
            }
        }

        // -----------------------------------------------------
        private string _City = "";
        public string City
        {
            get { return _City; }
            set
            {
                _City = value;
                OnPropertyChanged(new PropertyChangedEventArgs("City"));
            }
        }

        // -----------------------------------------------------
        private string _State = "";
        public string State
        {
            get { return _State; }
            set
            {
                _State = value;
                OnPropertyChanged(new PropertyChangedEventArgs("State"));
            }
        }

        // -----------------------------------------------------
        private string _Zipcode = "";
        public string Zipcode
        {
            get { return _Zipcode; }
            set
            {
                _Zipcode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Zipcode"));
            }
        }

        // -----------------------------------------------------
        private string _Phone = "";
        public string Phone
        {
            get { return _Phone; }
            set
            {
                _Phone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Phone"));
            }
        }

        // -----------------------------------------------------
        private string _Age = "";
        public string Age
        {
            get { return _Age; }
            set
            {
                _Age = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Age"));
            }
        }

        // -----------------------------------------------------
        private string _Interests = "";
        public string Interests
        {
            get { return _Interests; }
            set
            {
                _Interests = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Interests"));
            }
        }

        // -----------------------------------------------------
        private string _PhotoURL = "";
        public string PhotoURL
        {
            get { return _PhotoURL; }
            set
            {
                _PhotoURL = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PhotoURL"));
            }
        }
    }
}
