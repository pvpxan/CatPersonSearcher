using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CatPersonSearcher
{
    public class SettingsViewModel : DialogBaseWindowViewModel
    {
        // ViewModel Only Vars
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        private string defaultSearch = Config.Read("default_search").ToLower();

        // Constructor
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        public SettingsViewModel(DialogData data) : base(data)
        {
            Reset = new RelayCommand(resetCommand);
            SaveExit = new RelayCommand(saveExitCommand);
            Exit = new RelayCommand(exitCommand);

            switch (defaultSearch)
            {
                case "detailed":
                    DetailedSearchChecked = true;
                    break;

                case "quick":
                default:
                    QuickSearchChecked = true;
                    break;
            }

            if (bool.TryParse(Config.Read("show_all"), out bool showAll))
            {
                ShowAllRecordsCheckbox = showAll;
            }

            if (bool.TryParse(Config.Read("simulate_delay"), out bool simulateDelay))
            {
                SimulateDelayCheckBox = simulateDelay;
            }

            if (int.TryParse(Config.Read("simulate_delay_load"), out int loadDelay))
            {
                LoadDelayTextBox = Convert.ToString(loadDelay);
            }

            if (int.TryParse(Config.Read("simulate_delay_search"), out int searchDelay))
            {
                SearchDelayTextBox = Convert.ToString(searchDelay);
            }

            applySettings();
        }

        private void applySettings()
        {
            Statics.DefaultSearch = defaultSearch;
            Statics.ShowAll = ShowAllRecordsCheckbox;
            Statics.SimulateDelay = SimulateDelayCheckBox;

            if (int.TryParse(LoadDelayTextBox, out int loadDelay))
            {
                Statics.LoadDelay = loadDelay;
            }

            if (int.TryParse(SearchDelayTextBox, out int searchDelay))
            {
                Statics.SearchDelay = searchDelay;
            }
        }

        private void saveSettings()
        {
            if (QuickSearchChecked)
            {
                defaultSearch = "quick";
            }

            if (DetailedSearchChecked)
            {
                defaultSearch = "detailed";
            }

            Config.Update("default_search", defaultSearch);
            Config.Update("show_all", Convert.ToString(ShowAllRecordsCheckbox).ToLower());
            Config.Update("simulate_delay", Convert.ToString(SimulateDelayCheckBox).ToLower());
            Config.Update("simulate_delay_load", LoadDelayTextBox);
            Config.Update("simulate_delay_search", SearchDelayTextBox);

            applySettings();

        }

        // Bound Variables
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        private bool _QuickSearchChecked = false;
        public bool QuickSearchChecked
        {
            get
            {
                return _QuickSearchChecked;
            }
            set
            {
                _QuickSearchChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("QuickSearchChecked"));

                if (QuickSearchChecked)
                {

                }
            }
        }

        // -----------------------------------------------------
        private bool _DetailedSearchChecked = false;
        public bool DetailedSearchChecked
        {
            get
            {
                return _DetailedSearchChecked;
            }
            set
            {
                _DetailedSearchChecked = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DetailedSearchChecked"));
            }
        }

        // -----------------------------------------------------
        private bool _ShowAllRecordsCheckbox;
        public bool ShowAllRecordsCheckbox
        {
            get
            {
                return _ShowAllRecordsCheckbox;
            }
            set
            {
                _ShowAllRecordsCheckbox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowAllRecordsCheckbox"));
            }
        }

        // -----------------------------------------------------
        private bool _SimulateDelayCheckBox;
        public bool SimulateDelayCheckBox
        {
            get
            {
                return _SimulateDelayCheckBox;
            }
            set
            {
                _SimulateDelayCheckBox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SimulateDelayCheckBox"));
            }
        }

        // -----------------------------------------------------
        private string _LoadDelayTextBox;
        public string LoadDelayTextBox
        {
            get
            {
                return _LoadDelayTextBox;
            }
            set
            {
                _LoadDelayTextBox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LoadDelayTextBox"));
            }
        }

        // -----------------------------------------------------
        private string _SearchDelayTextBox;
        public string SearchDelayTextBox
        {
            get
            {
                return _SearchDelayTextBox;
            }
            set
            {
                _SearchDelayTextBox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SearchDelayTextBox"));
            }
        }

        // Bound Commands
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // ------------------------------------------------------
        public ICommand Reset { get; private set; }
        private void resetCommand(object parameter)
        {
            WindowMessageResult resetResult = MessageBoxEnhanced.Show(
                "Warning...",
                "You are about to reset the database!",
                "Any changes you have made to the current database will lost. Are you sue you wish to continue?",
                WindowMessageButtons.YesNo,
                WindowMessageIcon.Warning);

            if (resetResult == WindowMessageResult.Yes)
            {
                SQLConnection.Close();
                Statics.LoadResourceDatabase(true);
                CloseDialogWithResult(WindowMessageResult.Yes);
            }
        }

        // ------------------------------------------------------
        public ICommand SaveExit { get; private set; }
        private void saveExitCommand(object parameter)
        {
            saveSettings();
            CloseDialogWithResult(WindowMessageResult.Accept);
        }

        // ------------------------------------------------------
        public ICommand Exit { get; private set; }
        private void exitCommand(object parameter)
        {
            CloseDialogWithResult(WindowMessageResult.Exit);
        }
    }
}
