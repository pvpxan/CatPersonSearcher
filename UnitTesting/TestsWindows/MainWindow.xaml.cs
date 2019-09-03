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

namespace UnitTesting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void showRecords(string message)
        {
            SQLFunctions sqlFunctions = new SQLFunctions(SQLConnection.DatabaseContext);
            List<MultiModel> multiModelList = sqlFunctions.GetFullResults(0, SQLConnection.ReadPersonTableCount(true)); // Should get it all.
            int maxIndex = multiModelList.Count - 1;

            output.Text =
                output.Text + message +
                Environment.NewLine +
                "First:" + multiModelList[0].Firstname + " | " +
                "Last:" + multiModelList[0].Lastname + " | " +
                "First:" + multiModelList[0].City + " | " +
                "First:" + multiModelList[0].Age + " | " +
                Environment.NewLine +
                "First:" + multiModelList[maxIndex].Firstname + " | " +
                "Last:" + multiModelList[maxIndex].Lastname + " | " +
                "First:" + multiModelList[maxIndex].City + " | " +
                "First:" + multiModelList[maxIndex].Age + " | " +
                Environment.NewLine + Environment.NewLine;
        }

        private void Testing1_Click(object sender, RoutedEventArgs e)
        {
            SQLConnection.Initialize(Statics.DatabaseFile);

            output.Text =
                output.Text +
                "Initialized Database" +
                Environment.NewLine + Environment.NewLine;
        }

        private void Testing2_Click(object sender, RoutedEventArgs e)
        {
            SQLConnection.Close();
            Statics.LoadResourceDatabase(true);
            SQLConnection.Initialize(Statics.DatabaseFile);
            output.Text =
                output.Text +
                "Refreshed Database" +
                Environment.NewLine + Environment.NewLine;
        }

        private void Testing3_Click(object sender, RoutedEventArgs e)
        {
            DataBaseTable dataBaseTable = new DataBaseTable();
            dataBaseTable.ShowDialog();
            showRecords("Fresh.");
        }

        private void Testing4_Click(object sender, RoutedEventArgs e)
        {
            SQLFunctions sqlFunctions = new SQLFunctions(SQLConnection.DatabaseContext);
            List<MultiModel> multiModelList = sqlFunctions.GetFullResults(0, 100); // Should get it all.

            multiModelList[0].Firstname = "Test Firstname";
            multiModelList[0].City = "Test City";
            multiModelList[0].Age = "99";

            sqlFunctions.UpdateData(multiModelList[0]);
            showRecords("Update");
        }

        private void Testing5_Click(object sender, RoutedEventArgs e)
        {
            SQLFunctions sqlFunctions = new SQLFunctions(SQLConnection.DatabaseContext);
            MultiModel multiModel = new MultiModel()
            {
                PersonDataTable = new PersonData(),
                AddressDataTable = new AddressData(),
                DetailsDataTable = new DetailsData(),
            };

            multiModel.Firstname = "Test Add";
            multiModel.Lastname = "Last";
            multiModel.City = "Some Test City";
            multiModel.Age = "100";

            sqlFunctions.AddData(multiModel);
            showRecords("Add");
        }

        private void Testing6_Click(object sender, RoutedEventArgs e)
        {
            SQLFunctions sqlFunctions = new SQLFunctions(SQLConnection.DatabaseContext);
            List<MultiModel> multiModelList = sqlFunctions.GetFullResults(0, 100); // Should get it all.

            sqlFunctions.DeleteData(multiModelList[0]);
            showRecords("Delete");
        }

        private void Testing7_Click(object sender, RoutedEventArgs e)
        {
            SQLConnection.Close();
            SQLConnection.Initialize(Statics.DatabaseFile);
            output.Text =
                output.Text +
                "Reloaded" +
                Environment.NewLine + Environment.NewLine;
        }
    }
}
