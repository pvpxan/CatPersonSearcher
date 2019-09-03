using Microsoft.Win32;
using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CatPersonSearcher
{
    public class AddEditViewModel : DialogBaseWindowViewModel
    {
        // ViewModel Only Vars
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        private enum SQLFunctionType
        {
            Add,
            Modify,
            Delete,
        }

        private MultiModel PersonDataResults = null;

        // Constructor
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        public AddEditViewModel(DialogData data) : base(data)
        {
            Modify = new RelayCommand(modifyCommand);
            Clear = new RelayCommand(clearCommand);
            Browse = new RelayCommand(browseCommand);
            Catalog = new RelayCommand(catalogCommand);
            Delete = new RelayCommand(deleteCommand);

            // This is an edit window if we are passed in data.
            if (data.Parameter1 is MultiModel)
            {
                try
                {
                    ModifyContent = "Modify";
                    PersonDataResults = data.Parameter1 as MultiModel;

                    Firstname = PersonDataResults.Firstname;
                    Lastname = PersonDataResults.Lastname;
                    Street = PersonDataResults.Street;
                    City = PersonDataResults.City;
                    State = PersonDataResults.State;
                    Zipcode = PersonDataResults.Zipcode;
                    Phone = PersonDataResults.Phone;
                    Age = PersonDataResults.Age;
                    Interests = PersonDataResults.Interests;

                    loadImage(PersonDataResults.PhotoURL);
                }
                catch (Exception Ex)
                {
                    LogWriter.Exception("Error loading in person data from parameter.", Ex);
                }
            }
            else
            {
                addNewPerson();
            }
        }

        private void addNewPerson()
        {
            PersonDataResults = new MultiModel()
            {
                PersonDataTable = new PersonData(),
                AddressDataTable = new AddressData(),
                DetailsDataTable = new DetailsData(),
            };
        }

        private bool dataModified()
        {
            return 
                (Firstname != PersonDataResults.Firstname ||
                Lastname != PersonDataResults.Lastname ||
                Street != PersonDataResults.Street ||
                City != PersonDataResults.City ||
                State != PersonDataResults.State ||
                Zipcode != PersonDataResults.Zipcode ||
                Phone != PersonDataResults.Phone ||
                Age != PersonDataResults.Age ||
                Interests != PersonDataResults.Interests ||
                PhotoURL != PersonDataResults.PhotoURL);
        }

        private void modifyEntity()
        {
            PersonDataResults.Firstname = Firstname;
            PersonDataResults.Lastname = Lastname;
            PersonDataResults.Street = Street;
            PersonDataResults.City = City;
            PersonDataResults.State = State;
            PersonDataResults.Zipcode = Zipcode;
            PersonDataResults.Phone = Phone;
            PersonDataResults.Age = Age;
            PersonDataResults.Interests = Interests;
            PersonDataResults.PhotoURL = PhotoURL;
        }

        private async void loadImage(string imagePath)
        {
            ProgressBarIsEnabled = true;
            ProgressBarVisibility = Visibility.Visible;

            try
            {
                BitmapImage bitmapImage = null;
                Task taskLoadImage = null;
                taskLoadImage = Task.Run(() =>
                {
                    bitmapImage = ImageSourceReader.Read(imagePath);
                    if (bitmapImage != null)
                    {
                        PhotoURL = imagePath;
                    }
                });
                await taskLoadImage;
                if (taskLoadImage != null)
                {
                    taskLoadImage.Dispose();
                }

                Portrait = bitmapImage;
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Image task error.", Ex);
            }

            ProgressBarVisibility = Visibility.Hidden;
            ProgressBarIsEnabled = false;
        }

        // Bound Variables
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        private Visibility _ProgressBarVisibility = Visibility.Hidden;
        public Visibility ProgressBarVisibility
        {
            get { return _ProgressBarVisibility; }
            set
            {
                _ProgressBarVisibility = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProgressBarVisibility"));
            }
        }

        // -----------------------------------------------------
        private bool _ProgressBarIsEnabled = false;
        public bool ProgressBarIsEnabled
        {
            get { return _ProgressBarIsEnabled; }
            set
            {
                _ProgressBarIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProgressBarIsEnabled"));
            }
        }

        // -----------------------------------------------------
        private string _ModifyContent = "Add";
        public string ModifyContent
        {
            get { return _ModifyContent; }
            set
            {
                _ModifyContent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ModifyContent"));
            }
        }

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

        // -----------------------------------------------------
        private BitmapImage _Portrait = null;
        public BitmapImage Portrait
        {
            get { return _Portrait; }
            set
            {
                _Portrait = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Portrait"));

                ImageSourceReader.Cleanup();
            }
        }

        // Bound ICommands
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        public ICommand Browse { get; private set; }
        private void browseCommand(object parameter)
        {
            OpenFileDialog openFilesDialog = new OpenFileDialog();
            openFilesDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFilesDialog.Multiselect = false;
            openFilesDialog.Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif";

            if (openFilesDialog.ShowDialog() == true)
            {
                loadImage(openFilesDialog.FileName);
            }
        }

        // -----------------------------------------------------
        public ICommand Catalog { get; private set; }
        private void catalogCommand(object parameter)
        {
            // Had more plans for this, but oh well.
            // Used if you want to get the local resource name for a photo.
            WindowFactory.OpenWindow(new WindowFactoryData() { WindowType = WindowClassType.Catalog, Modal = true, });
        }

        // -----------------------------------------------------
        public ICommand Clear { get; private set; }
        private void clearCommand(object parameter)
        {
            addNewPerson();
            ModifyContent = "Add";

            addNewPerson();
            Firstname = "";
            Lastname = "";
            Street = "";
            City = "";
            State = "";
            Zipcode = "";
            Phone = "";
            Age = "";
            Interests = "";
            PhotoURL = "";
            Portrait = null;
        }

        // -----------------------------------------------------
        public ICommand Modify { get; private set; }
        private void modifyCommand(object parameter)
        {
            if (string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Lastname))
            {
                return;
            }

            if (ModifyContent.ToLower() == "add")
            {
                modifyEntity();
                runTask(SQLFunctionType.Add);
            }
            else
            {
                if (dataModified() == false)
                {
                    return;
                }

                WindowMessageResult modifyResult = MessageBoxEnhanced.Show(
                    "Question...",
                    "You are about to modify a record in the database! Do you wish to continue?",
                    WindowMessageButtons.YesNo,
                    WindowMessageIcon.Question);

                if (modifyResult == WindowMessageResult.Yes)
                {
                    modifyEntity();
                    runTask(SQLFunctionType.Modify);
                }
            }
        }

        // -----------------------------------------------------
        public ICommand Delete { get; private set; }
        private void deleteCommand(object parameter)
        {
            WindowMessageResult deleteResult = MessageBoxEnhanced.Show(
                "Question...",
                "You are about to delete a record from the database! Do you wish to continue?",
                WindowMessageButtons.YesNo,
                WindowMessageIcon.Question);

            if (deleteResult == WindowMessageResult.Yes)
            {
                runTask(SQLFunctionType.Delete);
            }
        }

        private async void runTask(SQLFunctionType sqlFunctionType)
        {
            if (ProgressBarIsEnabled)
            {
                return;
            }

            ProgressBarIsEnabled = true;
            ProgressBarVisibility = Visibility.Visible;

            try
            {
                Task task = null;
                task = Task.Run(() =>
                {
                    SQLFunctions sqlFunctions = new SQLFunctions(SQLConnection.DatabaseContext);

                    switch (sqlFunctionType)
                    {
                        case SQLFunctionType.Add:
                            sqlFunctions.AddData(PersonDataResults);
                            break;

                        case SQLFunctionType.Modify:
                            sqlFunctions.UpdateData(PersonDataResults);
                            break;

                        case SQLFunctionType.Delete:
                            sqlFunctions.DeleteData(PersonDataResults);
                            break;
                    }
                });
                await task;
                if (task != null)
                {
                    task.Dispose();
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Image task error.", Ex);
            }

            ProgressBarVisibility = Visibility.Hidden;
            ProgressBarIsEnabled = false;

            CloseDialogWithResult(WindowMessageResult.Accept);
        }
    }
}
