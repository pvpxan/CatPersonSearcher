using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CatPersonSearcher
{
    public partial class SearchViewModel : ViewModelBase
    {
        // Bound ICommands
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        public ICommand DetailedSearch { get; private set; }
        private void detailedSearchCommand(object parameter)
        {
            if (quickSearchParam == false)
            {
                cancelTasks();

                if (SearchTextbox.Length != 1 && SearchTextbox != currentSearch)
                {
                    SearchTextboxChanged(SearchTextbox);
                }
            }
        }

        // -----------------------------------------------------
        public ICommand PageBack { get; private set; }
        private void pageBackCommand(object parameter)
        {
            querySQL(PageCurrent - 1);
        }
        // -----------------------------------------------------
        public ICommand GoToPage { get; private set; }
        private void goToPageCommand(object parameter)
        {
            if (PageCount < 2 ||
                int.TryParse(PageTextbox, out int pageNumber) == false ||
                pageNumber == PageCurrent ||
                pageNumber > PageCount)
            {
                PageTextBoxIsReadOnly = true; // Part of a super janky hack to get the caret postion in the right spot.
                PageTextbox = Convert.ToString(PageCurrent);
                PageTextBoxIsReadOnly = false;

                return;
            }

            querySQL(pageNumber);
        }

        // -----------------------------------------------------
        public ICommand PageNext { get; private set; }
        private void pageNextCommand(object parameter)
        {
            querySQL(PageCurrent + 1);
        }

        // -----------------------------------------------------
        public ICommand Edit { get; private set; }
        private void editCommand(object parameter)
        {
            if (ProgressBarIsEnabled || SelectedIndex < 0)
            {
                return;
            }
            
            DialogData modifyData = new DialogData();
            modifyData.WindowTitle = "Modify Database...";
            modifyData.Topmost = false;
            modifyData.Background = ColorSets.HexConverter("#FF1E1E1E");
            modifyData.Parameter1 = PersonTableResults[SelectedIndex];
            DialogBaseWindowViewModel viewmodel = new AddEditViewModel(modifyData);
            WindowMessageResult settingsResult = DialogService.OpenDialog(viewmodel, parameter as Window, ShutdownMode.OnLastWindowClose);
            
            // Data was added and the view needs to reloaded.
            // Sloppy and could be better by updating page counts or adding to the existing page if it matches the current search pattern.
            if (settingsResult == WindowMessageResult.Accept)
            {
                querySQL(PageCurrent);
            }
        }

        // -----------------------------------------------------
        public ICommand Add { get; private set; }
        private void addCommand(object parameter)
        {
            DialogData modifyData = new DialogData();
            modifyData.WindowTitle = "Modify Database...";
            modifyData.Topmost = false;
            modifyData.Background = ColorSets.HexConverter("#FF1E1E1E");

            DialogBaseWindowViewModel viewmodel = new AddEditViewModel(modifyData);
            WindowMessageResult settingsResult = DialogService.OpenDialog(viewmodel, parameter as Window, ShutdownMode.OnLastWindowClose);

            // Data was deleted and the view needs to reloaded.
            // Sloppy and could be better by updating page counts or adding to the existing page if it matches the current search pattern.
            if (settingsResult == WindowMessageResult.Accept)
            {
                querySQL(PageCurrent);
            }
        }

        // -----------------------------------------------------
        public ICommand Refresh { get; private set; }
        private void refreshCommand(object parameter)
        {
            cancelTasks();

            PageCount = 0;
            PageCurrent = 0;
            PersonTableResults.Clear();

            refreshContext();
        }

        private async void refreshContext()
        {
            try
            {
                Task refreshContextTask = null;
                refreshContextTask = Task.Run(() =>
                {
                    simulatedDelay();

                    SQLConnection.Close();
                    SQLConnection.Initialize(Statics.DatabaseFile);
                });
                await refreshContextTask;

                if (refreshContextTask != null)
                {
                    refreshContextTask.Dispose();
                }

                querySQL(1);
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Bummer. That search delay broke somehow.", Ex);
            }
        }
    }
}
