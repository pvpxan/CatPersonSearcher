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
    public partial class MainWindowViewModel : ViewModelBase
    {
        // ViewModel Only Vars
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        

        // Constructor
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        public MainWindowViewModel()
        {
            Loaded = new RelayCommand(loadedCommand);
            Settings = new RelayCommand(settingsCommand);
            OpenCatalog = new RelayCommand(openCatalogCommand);
            About = new RelayCommand(aboutCommand);
            ToggleSearch = new RelayCommand(toggleSearchCommand);
        }

        // Bound ICommands
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        public ICommand Loaded { get; private set; }
        private void loadedCommand(object parameter)
        {
            // Not currently used.
        }

        // -----------------------------------------------------
        public ICommand Settings { get; private set; }
        private void settingsCommand(object parameter)
        {
            DialogData settingsData = new DialogData();
            settingsData.WindowTitle = "Settings...";
            settingsData.Topmost = false;
            settingsData.Background = ColorSets.HexConverter("#FF1E1E1E");

            DialogBaseWindowViewModel viewmodel = new SettingsViewModel(settingsData);
            WindowMessageResult settingsResult = DialogService.OpenDialog(viewmodel, parameter as Window, ShutdownMode.OnLastWindowClose);

            if (settingsResult == WindowMessageResult.Yes)
            {
                ChangeContent.Select(MainContent.Connect);
            }
        }

        // -----------------------------------------------------
        public ICommand OpenCatalog { get; private set; }
        private void openCatalogCommand(object parameter)
        {
            WindowFactory.OpenWindow(new WindowFactoryData() { WindowType = WindowClassType.Catalog, Modal = false, });
        }

        // -----------------------------------------------------
        public ICommand About { get; private set; }
        private void aboutCommand(object parameter)
        {
            DialogData aboutdata = new DialogData();
            aboutdata.WindowTitle = "Information...";
            aboutdata.MessageIcon = WindowMessageIcon.Information;
            aboutdata.MessageButtons = WindowMessageButtons.Ok;
            aboutdata.ContentHeader = "Cat Person Searcher";
            aboutdata.ContentBody =
                "Version: " + Convert.ToString(Statics.CurrentFileVersion) + Environment.NewLine +
                "Me ©2019" + Environment.NewLine +
                "518-524-3706";

            aboutdata.HyperLinkUri = "https://github.com/pvpxan";
            aboutdata.HyperLinkText = "https://github.com/pvpxan";

            DialogService.OpenWindowMessage(aboutdata, this);
        }

        // -----------------------------------------------------
        public ICommand ToggleSearch { get; private set; }
        private void toggleSearchCommand(object parameter)
        {
            ChangeContent.Select(MainContent.Toggle);
        }
    }
}
