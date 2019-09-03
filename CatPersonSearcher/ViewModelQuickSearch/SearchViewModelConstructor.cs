using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CatPersonSearcher
{
    public partial class SearchViewModel : ViewModelBase
    {
        // ViewModel Only Vars
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        bool quickSearchParam = true;

        // Constructor
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        public SearchViewModel(bool quickSearch)
        {
            quickSearchParam = quickSearch;

            DetailedSearch = new RelayCommand(detailedSearchCommand);
            Edit = new RelayCommand(editCommand);
            Refresh = new RelayCommand(refreshCommand);
            GoToPage = new RelayCommand(goToPageCommand);
            PageBack = new RelayCommand(pageBackCommand);
            PageNext = new RelayCommand(pageNextCommand);
            Add = new RelayCommand(addCommand);

            configurePopulateTask();
            configureGetDataTask();
            querySQL(1);
        }

        private void cancelTasks()
        {
            if (populateListViewTask.CancellationRequested == false)
            {
                populateListViewTask.CancelTask();
                populateListViewTask.Dispose();
                configurePopulateTask();
            }

            cancelGetData();
        }

        private void cancelGetData()
        {
            if (getDataTask.CancellationRequested == false)
            {
                getDataTask.CancelTask();
                getDataTask.Dispose();
                configureGetDataTask();
            }
        }

        private void configurePopulateTask()
        {
            populateListViewTask = new TaskWorker();
            populateListViewTask.TaskAction += populateTaskStart;
            populateListViewTask.TaskComplete += populateTaskComplete;
        }

        private void configureGetDataTask()
        {
            getDataTask = new TaskWorker();
            getDataTask.TaskAction += getDataTaskStart;
            getDataTask.TaskComplete += getDataTaskComplete;
        }

        private void querySQL(int page)
        {
            if (SearchTextbox.Length < 2 && Statics.ShowAll == false)
            {
                return;
            }

            ProgressBarIsEnabled = true;
            ProgressBarVisibility = Visibility.Visible;

            Tuple<string, int> taskData = new Tuple<string, int>(SearchTextbox, page);
            populateListViewTask.StartTask(taskData);
        }

        private void getPersonData(int index)
        {
            ProgressBarIsEnabled = true;
            ProgressBarVisibility = Visibility.Visible;
            getDataTask.StartTask(index);
        }

        private async void SearchTextboxChanged(string lastSearchTextbox)
        {
            try
            {
                Task taskDelay = null;
                taskDelay = Task.Delay(1000);
                await taskDelay;

                if (taskDelay != null)
                {
                    taskDelay.Dispose();
                }

                if (SearchTextbox == lastSearchTextbox)
                {
                    querySQL(1);
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Bummer. That search delay broke somehow.", Ex);
            }
        }

        // Simulated delay.
        private void simulatedDelay()
        {
            if (Statics.SimulateDelay == false)
            {
                return;
            }

            int delay = Statics.SearchDelay * 10;
            int count = 0;
            while (count < delay && populateListViewTask.CancellationRequested == false)
            {
                count++;
                Thread.Sleep(100);
            }
        }
    }
}
