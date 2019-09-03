using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace UnitTesting
{
    /// <summary>
    /// Interaction logic for DataBaseTable.xaml
    /// </summary>
    public partial class DataBaseTable : Window
    {
        public DataBaseTable()
        {
            InitializeComponent();
            this.DataContext = new DataBaseTableViewModel();
        }
    }

    public class DataBaseTableViewModel : ViewModelBase
    {
        public DataBaseTableViewModel()
        {
            try
            {
                SQLFunctions sqlFunctions = new SQLFunctions(SQLConnection.DatabaseContext);
                List<MultiModel> multiModelList = sqlFunctions.GetFullResults(0, 100); // Should get it all.

                foreach (var item in multiModelList)
                {
                    AllTablesResults.Add(new AllTablesModel()
                    {
                        Firstname = item.Firstname,
                        Lastname = item.Lastname,
                        Street = item.Street,
                        City = item.City,
                        State = item.State,
                        Zipcode = item.Zipcode,
                        Phone = item.Phone,
                        Age = item.Age,
                        Interests = item.Interests,
                        PhotoURL = item.PhotoURL
                    });
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        // -----------------------------------------------------
        private ObservableCollection<AllTablesModel> _AllTablesResults = new ObservableCollection<AllTablesModel>();
        public ObservableCollection<AllTablesModel> AllTablesResults
        {
            get { return _AllTablesResults; }
            set
            {
                _AllTablesResults = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AllTablesResults"));
            }
        }






    }
}
