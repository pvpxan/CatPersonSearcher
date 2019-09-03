using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CatPersonSearcher
{
    // Wrapper class for TaskFactory wipped together to sort of simulate BackgroundWorker class but use the better Tasks technology.
    // Just mostly trying this out.
    public class TaskWorkerEventArgs
    {
        public object Parameters { get; set; }
        public object Results { get; set; }
        public bool CancellationRequested { get; set; } = false;
        public bool Cancelled { get; set; } = false;
    }

    public class TaskWorker : IDisposable
    {
        private Task task = null;
        private CancellationTokenSource source = null;
        private CancellationToken token;
        private TaskWorkerEventArgs taskWorkerEventArgs = new TaskWorkerEventArgs();

        public Action<object, TaskWorkerEventArgs> TaskAction { get; set; }
        public Action<object, TaskWorkerEventArgs> TaskComplete { get; set; }

        private bool _CancellationRequested = false;
        public bool CancellationRequested
        {
            get
            {
                return _CancellationRequested;
            }
        }

        private bool _IsBusy = false;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
        }

        public void StartTask()
        {
            runTask(null);
        }

        public void StartTask(object paramaters)
        {
            runTask(paramaters);
        }
        
        private void runTask(object paramaters)
        {
            if (_IsBusy || TaskAction == null || TaskComplete == null)
            {
                // This should probrably throw or something else.
                return;
            }

            Dispose(); // If we are reusing an instance of this class, we want to clean it up first.
            source = new CancellationTokenSource();

            try
            {
                token = source.Token;
                _IsBusy = true;

                taskWorkerEventArgs.Parameters = paramaters;
                task = Task.Factory.
                    StartNew(() => TaskAction(this, taskWorkerEventArgs), token, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).
                    ContinueWith((t) => taskComplete(), TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch
            {
                // Nothing really needs to be here.
            }
        }

        public void CancelTask()
        {
            if (source != null)
            {
                source.Cancel();
                _CancellationRequested = true;
            }

            taskWorkerEventArgs.Cancelled = true;
            taskComplete();
        }

        // Completed event that runs on the initial task start calling thread.
        public void taskComplete()
        {
            if (IsBusy == false) // If the IsBusy is false, we just want to fizzle out. This means the thread was cancelled already.
            {
                return;
            }
            _IsBusy = false;

            TaskComplete(this, taskWorkerEventArgs);

            Dispose();
        }

        // Although this class method internally cleans up it is still good to expose this method just in case.
        public void Dispose()
        {
            try
            {
                if (task != null)
                {
                    task.Dispose();
                    task = null;
                }

                if (source != null)
                {
                    source.Dispose();
                    source = null;
                }
            }
            catch
            {
                // The above may fail is the task is somehow dead locked. Will look into this more.
            }

            taskWorkerEventArgs = new TaskWorkerEventArgs();
        }
    }
}
