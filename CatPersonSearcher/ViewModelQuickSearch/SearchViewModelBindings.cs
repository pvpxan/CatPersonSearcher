using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CatPersonSearcher
{
    public partial class SearchViewModel : ViewModelBase
    {
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
        private int _SelectedIndex = -1;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SelectedIndex"));

                if (quickSearchParam)
                {
                    if (SelectedIndex > -1)
                    {
                        cancelGetData();
                        getPersonData(SelectedIndex);
                    }
                    else
                    {
                        PersonDataResults = null;
                        Portrait = null;
                    }
                }
            }
        }

        // -----------------------------------------------------
        private ObservableCollection<MultiModel> _PersonTableResults = new ObservableCollection<MultiModel>();
        public ObservableCollection<MultiModel> PersonTableResults
        {
            get { return _PersonTableResults; }
            set
            {
                _PersonTableResults = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PersonTableResults"));
            }
        }

        // -----------------------------------------------------
        private MultiModel _PersonDataResults;
        public MultiModel PersonDataResults
        {
            get { return _PersonDataResults; }
            set
            {
                _PersonDataResults = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PersonDataResults"));
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

        // -----------------------------------------------------
        private string _SearchTextbox = "";
        public string SearchTextbox
        {
            get { return _SearchTextbox; }
            set
            {
                _SearchTextbox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SearchTextbox"));

                if (quickSearchParam)
                {
                    cancelTasks();

                    if (SearchTextbox.Length != 1 && SearchTextbox != currentSearch)
                    {
                        SearchTextboxChanged(SearchTextbox);
                    }
                }
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
    }
}
