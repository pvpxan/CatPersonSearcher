using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    // MultiModel
    public partial class MultiModel : ViewModelBase
    {
        public long ID { get; set; }

        // --------------------------------------------------------------------------------------
        public PersonData PersonDataTable { get; set; } = new PersonData();
        public long PersonID
        {
            get { return PersonDataTable.ID; }
            set { PersonDataTable.ID = value; }
        }
        public string Firstname
        {
            get { return PersonDataTable.Firstname; }
            set
            {
                PersonDataTable.Firstname = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Firstname"));
            }
        }
        public string Lastname
        {
            get { return PersonDataTable.Lastname; }
            set
            {
                PersonDataTable.Lastname = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Lastname"));
            }
        }

        // --------------------------------------------------------------------------------------
        public AddressData AddressDataTable { get; set; } = new AddressData();
        public string Street
        {
            get { return AddressDataTable.Street; }
            set
            {
                AddressDataTable.Street = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Street"));
            }
        }
        public string City
        {
            get { return AddressDataTable.City; }
            set
            {
                AddressDataTable.City = value;
                OnPropertyChanged(new PropertyChangedEventArgs("City"));
            }
        }
        public string State
        {
            get { return AddressDataTable.State; }
            set
            {
                AddressDataTable.State = value;
                OnPropertyChanged(new PropertyChangedEventArgs("State"));
            }
        }
        public string Zipcode
        {
            get { return AddressDataTable.Zipcode; }
            set
            {
                AddressDataTable.Zipcode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Zipcode"));
            }
        }
        public string Phone
        {
            get { return AddressDataTable.Phone; }
            set
            {
                AddressDataTable.Phone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Phone"));
            }
        }

        // --------------------------------------------------------------------------------------
        public DetailsData DetailsDataTable { get; set; } = new DetailsData();
        public string Age
        {
            get { return DetailsDataTable.Age; }
            set
            {
                DetailsDataTable.Age = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Age"));
            }
        }
        public string Interests
        {
            get { return DetailsDataTable.Interests; }
            set
            {
                DetailsDataTable.Interests = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Interests"));
            }
        }
        public string PhotoURL
        {
            get { return DetailsDataTable.PhotoURL; }
            set
            {
                DetailsDataTable.PhotoURL = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PhotoURL"));
            }
        }
    }
}
