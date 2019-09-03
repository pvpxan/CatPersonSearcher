using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CatPersonSearcher
{
    // Event driven window creation.

    // Friendly names for use in switch case to tell the factory what window to open.
    public enum WindowClassType
    {
        MainWindow,
        Catalog,
        Undefined,
    }

    // Data sent by ViewModel when a window needs to open.
    public class WindowFactoryData
    {
        public ViewModelBase CallingViewModel { get; set; } = null;
        public WindowClassType WindowType { get; set; } = WindowClassType.Undefined;
        public bool Modal { get; set; } = false;
        public object DataAgggregator { get; set; } = null;
    }

    // Registers a ViewModel with a corresponding window to allow for opening dialogs with a designated parent.
    public static class WindowFactory
    {
        public static void OpenWindow(WindowFactoryData data)
        {
            if (OnDataTransmittedEvent != null)
            {
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        OnDataTransmittedEvent(data);
                    });
                }
                else
                {
                    OnDataTransmittedEvent(data);
                }
            }
        }
        public static Action<WindowFactoryData> OnDataTransmittedEvent { get; set; }

        public static void Initialize()
        {
            OnDataTransmittedEvent += openWindowHandler;
        }

        private static void openWindowHandler(WindowFactoryData data)
        {
            Window window = null;
            object viewModel = null;

            switch (data.WindowType)
            {
                case WindowClassType.MainWindow:
                    window = new MainWindow();
                    viewModel = new MainWindowViewModel();
                    break;

                case WindowClassType.Catalog:
                    window = new Catalog();
                    viewModel = new CatalogViewModel(data.DataAgggregator);
                    break;

                default:
                    break;
            }

            if (window == null || viewModel == null)
            {
                return;
            }

            window.DataContext = viewModel;
            FactoryService.Register(viewModel as ViewModelBase, window);

            // Attemps to find a parent window based on what ViewModel sent the request.
            Window parentWindow = FactoryService.GetWindowReference(data.CallingViewModel);
            if (parentWindow != null)
            {
                window.Owner = parentWindow;
            }

            if (data.Modal)
            {
                window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }
    }
}
