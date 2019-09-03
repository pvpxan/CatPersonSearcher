using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CatPersonSearcher
{
    public class CatalogViewModel : ViewModelBase
    {
        // ViewModel Only Vars
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        private readonly int defaultTake = 5; // Number of records per page we want to see.
        private int calculateSkip(int pageNumber)
        {
            return (pageNumber - 1) * defaultTake;
        }


        // Constructor
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        public CatalogViewModel(object dataAggregator)
        {
            GoToPage = new RelayCommand(goToPageCommand);
            PageBack = new RelayCommand(pageBackCommand);
            PageNext = new RelayCommand(pageNextCommand);

            getPage(1);
        }

        private void getPage(int page)
        {
            int imageCount = Statics.ResourceCatalog.Count(r => r.Contains("ResourceCats"));
            if (imageCount < 1 || page < 1)
            {
                return;
            }

            PageCount = ((imageCount - 1) / defaultTake) + 1; // The above checks ensures this does not go badly.
            PhotoThumbnails.Clear();
            ImageSourceReader.Cleanup();

            try
            {
                var imageFiles = Statics.ResourceCatalog.Where(
                    p => p.Contains("ResourceCats")).Skip(calculateSkip(page)).Take(defaultTake);

                foreach (string image in imageFiles)
                {
                    BitmapImage bitmapImage = ImageSourceReader.Read(image);
                    if (bitmapImage == null)
                    {
                        continue;
                    }

                    string[] imageNameSplit = image.Split('.');
                    string filename = "ResourceCats." + imageNameSplit[2] + ".jpg";

                    PhotoThumbnails.Add(new CatModel() { Filename = filename, Portrait = bitmapImage });
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error pulling catalog data. Please make sure there are no cats chewing on the cables behind your PC.", Ex);
            }

            PageCurrent = page;
        }

        // Bound Variables
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        private int _SelectedIndex = -1;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndex"));

                Portrait = null;
                Portrait = PhotoThumbnails[SelectedIndex].Portrait;
                Filename = PhotoThumbnails[SelectedIndex].Filename;
            }
        }

        // -----------------------------------------------------
        private ObservableCollection<CatModel> _PhotoThumbnails = new ObservableCollection<CatModel>();
        public ObservableCollection<CatModel> PhotoThumbnails
        {
            get { return _PhotoThumbnails; }
            set
            {
                _PhotoThumbnails = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PhotoThumbnails"));
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
                 
                if (Portrait == null)
                {
                    ImageSourceReader.Cleanup();
                }
            }
        }

        // -----------------------------------------------------
        private string _Filename = null;
        public string Filename
        {
            get { return _Filename; }
            set
            {
                _Filename = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Filename"));
            }
        }

        // -----------------------------------------------------
        private int _PageCount = 0;
        public int PageCount
        {
            get { return _PageCount; }
            set
            {
                _PageCount = value;
                PageCountLabel = Convert.ToString(PageCount);
                PageTextBoxIsReadOnly = PageCount > 1 == false;
            }
        }

        // -----------------------------------------------------
        private int _PageCurrent = 0;
        public int PageCurrent
        {
            get { return _PageCurrent; }
            set
            {
                _PageCurrent = value;

                PageTextbox = Convert.ToString(PageCurrent);
                PageBackIsEnabled = PageCurrent > 1;
                PageNextIsEnabled = PageCurrent < PageCount;
            }
        }

        // -----------------------------------------------------
        private string _PageCountLabel = "";
        public string PageCountLabel
        {
            get { return _PageCountLabel; }
            set
            {
                _PageCountLabel = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PageCountLabel"));
            }
        }

        // -----------------------------------------------------
        private string _PageTextbox = "";
        public string PageTextbox
        {
            get { return _PageTextbox; }
            set
            {
                _PageTextbox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PageTextbox"));
            }
        }

        // -----------------------------------------------------
        private bool _PageTextBoxIsReadOnly = true;
        public bool PageTextBoxIsReadOnly
        {
            get { return _PageTextBoxIsReadOnly; }
            set
            {
                _PageTextBoxIsReadOnly = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PageTextBoxIsReadOnly"));
            }
        }

        // -----------------------------------------------------
        private bool _PageBackIsEnabled = false;
        public bool PageBackIsEnabled
        {
            get { return _PageBackIsEnabled; }
            set
            {
                _PageBackIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PageBackIsEnabled"));
            }
        }

        // -----------------------------------------------------
        private bool _PageNextIsEnabled = false;
        public bool PageNextIsEnabled
        {
            get { return _PageNextIsEnabled; }
            set
            {
                _PageNextIsEnabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PageNextIsEnabled"));
            }
        }

        // Bound ICommands
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        // -----------------------------------------------------
        public ICommand PageBack { get; private set; }
        private void pageBackCommand(object parameter)
        {
            getPage(PageCurrent - 1);
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

            getPage(pageNumber);
        }

        // -----------------------------------------------------
        public ICommand PageNext { get; private set; }
        private void pageNextCommand(object parameter)
        {
            getPage(PageCurrent + 1);
        }
    }
}
