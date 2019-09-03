using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatPersonSearcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ChangeContent.OnChangeContentRequest += changeContect;
            SizeChanged += sizeChanged;
            Closed += closed;
        }

        private void closed(object sender, EventArgs e)
        {
            DLLEmbeddingHandler.Shutdown();
        }

        private void sizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Nothing right now as this does not seem to fire at the right time.
        }

        private void changeContect(MainContent mainContent)
        {
            UIElement uIElement = null;
            switch (mainContent)
            {
                case MainContent.Connect:
                    if (dynamicGrid.Children[0] is Connect)
                    {
                        return;
                    }

                    toggleMenuItem.IsEnabled = false;
                    toggleMenuItem.Visibility = Visibility.Hidden;
                    uIElement = new Connect();
                    break;

                case MainContent.Detailed:
                    if (dynamicGrid.Children[0] is DetailedSearch)
                    {
                        return;
                    }

                    toggleMenuItem.IsEnabled = true;
                    toggleMenuItem.Visibility = Visibility.Visible;
                    uIElement = new DetailedSearch(false);
                    break;

                case MainContent.Quick:
                    if (dynamicGrid.Children[0] is QuickSearch)
                    {
                        return;
                    }

                    toggleMenuItem.IsEnabled = true;
                    toggleMenuItem.Visibility = Visibility.Visible;
                    uIElement = new QuickSearch(true);
                    break;

                case MainContent.Toggle:
                    if (dynamicGrid.Children.Count < 1) // Should never happen.
                    {
                        uIElement = new QuickSearch(true);
                        break;
                    }

                    if (dynamicGrid.Children[0] is QuickSearch)
                    {
                        uIElement = new DetailedSearch(false);
                    }
                    else if (dynamicGrid.Children[0] is DetailedSearch)
                    {
                        uIElement = new QuickSearch(true);
                    }

                    break;
            }

            if (uIElement != null)
            {
                dynamicGrid.Children.Clear();
                dynamicGrid.Children.Add(uIElement);
                sizeChangeDelay();
            }
        }

        // Hack. Waits a moment for the FrameWork element to update the window size, then adjusts where it should be.
        private async void sizeChangeDelay()
        {
            try
            {
                Task taskDelay = null;
                taskDelay = Task.Delay(1);
                await taskDelay;

                double screenWidth = SystemParameters.PrimaryScreenWidth;
                double screenHeight = SystemParameters.PrimaryScreenHeight;
                double windowWidth = this.Width;
                double windowHeight = this.Height;
                this.Left = (screenWidth / 2) - (windowWidth / 2);
                this.Top = (screenHeight / 2) - (windowHeight / 2);

                if (taskDelay != null)
                {
                    taskDelay.Dispose();
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Bummer. That size change delay broke somehow.", Ex);
            }
        }
    }

    public enum MainContent
    {
        Connect,
        Detailed,
        Quick,
        Toggle,
    }

    public static class ChangeContent
    {
        public static void Select(MainContent data)
        {

            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    OnChangeContentRequest(data);
                });
            }
            else
            {
                OnChangeContentRequest(data);
            }
        }

        public static Action<MainContent> OnChangeContentRequest { get; set; }
    }
}
