using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CatPersonSearcher
{
    public class ConnectViewModel : ViewModelBase
    {
        // ViewModel Only Vars
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        private TaskWorker taskWorker = new TaskWorker();

        // Constructor
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        public ConnectViewModel()
        {
            Connect = new RelayCommand(connectCommand);

            taskWorker.TaskAction += connectTaskStart;
            taskWorker.TaskComplete += connectTaskComplete;
        }

        // Bound Variables
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        private string _ConnectButtonContent = "Connect to Database";
        public string ConnectButtonContent
        {
            get
            {
                return _ConnectButtonContent;
            }
            set
            {
                _ConnectButtonContent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectButtonContent"));
            }
        }

        // -----------------------------------------------------
        private string _ConnectMessage = "";
        public string ConnectMessage
        {
            get
            {
                return _ConnectMessage;
            }
            set
            {
                _ConnectMessage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectMessage"));
            }
        }

        // -----------------------------------------------------
        private bool _ConnectIsEnabled = true;
        public bool ConnectIsEnabled
        {
            get
            {
                return _ConnectIsEnabled;
            }
            set
            {
                _ConnectIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ConnectIsEnabled"));
            }
        }

        // -----------------------------------------------------
        private System.Windows.Visibility _ProgressBarVisibility = System.Windows.Visibility.Hidden;
        public System.Windows.Visibility ProgressBarVisibility
        {
            get
            {
                return _ProgressBarVisibility;
            }
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
            get
            {
                return _ProgressBarIsEnabled;
            }
            set
            {
                _ProgressBarIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ProgressBarIsEnabled"));
            }
        }

        // Bound ICommands
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        public ICommand Connect { get; set; }
        private void connectCommand(object parameter)
        {
            if (taskWorker.IsBusy)
            {
                taskWorker.CancelTask();
            }
            else
            {
                taskWorker.StartTask();
            }
        }

        private void connectTaskStart(object sender, TaskWorkerEventArgs e)
        {
            ConnectMessage = "Connecting to local database...";
            ConnectButtonContent = "Abort Connection";
            ProgressBarIsEnabled = true;
            ProgressBarVisibility = System.Windows.Visibility.Visible;

            simulatedDelay();

            if (taskWorker.CancellationRequested)
            {
                return;
            }

            // Connects to the Database.
            if (SQLConnection.Initialize(Statics.DatabaseFile))
            {
                e.Results = true;
            }
        }

        // Simulated delay.
        private void simulatedDelay()
        {
            if (Statics.SimulateDelay == false)
            {
                return;
            }

            int delay = Statics.LoadDelay * 10;
            int count = 0;
            int firstMessage = delay / 4;
            int seconMessage = delay / 2;

            while (count < delay && taskWorker.CancellationRequested == false)
            {
                if (count == firstMessage)
                {
                    ConnectMessage = "Buffering cat images...";
                }

                if (count == seconMessage)
                {
                    ConnectMessage = "Performing more time consuming tasks...";
                }

                count++;
                Thread.Sleep(100);
            }
        }

        private void connectTaskComplete(object sender, TaskWorkerEventArgs e)
        {
            ProgressBarVisibility = System.Windows.Visibility.Hidden;
            ProgressBarIsEnabled = false;

            bool success = false;
            if (e.Results != null && e.Results is bool)
            {
                success = (bool)e.Results;
            }

            if (success == false)
            {
                ConnectButtonContent = "Connect to Database";
                return;
            }

            ConnectIsEnabled = false;
            string searchDefault = Config.Read("default_search").ToLower(); ;
            switch (searchDefault)
            {
                case "detailed":
                    ChangeContent.Select(MainContent.Detailed);
                    break;

                case "quick":
                default:
                    ChangeContent.Select(MainContent.Quick);
                    break;
            }

            // We are done with this control.
        }
    }
}
