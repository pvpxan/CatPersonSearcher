using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CatPersonSearcher
{
    public partial class SearchViewModel : ViewModelBase
    {
        // ViewModel Only Vars
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        TaskWorker getDataTask;

        // ViewModel Search Worker
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        private void getDataTaskStart(object sender, TaskWorkerEventArgs e)
        {
            if (e.Parameters == null && e.Parameters is int == false)
            {
                return;
            }

            simulatedDelay();

            if (getDataTask.CancellationRequested)
            {
                return;
            }

            int index = (int)e.Parameters;
            SQLFunctions sqlFunctions = new SQLFunctions(SQLConnection.DatabaseContext);
            MultiModel displayModel = null;
            displayModel = sqlFunctions.GetPersonDetails(PersonTableResults[index].PersonID);

            if (getDataTask.CancellationRequested)
            {
                return;
            }

            if (displayModel != null)
            {
                BitmapImage bitmapImage = ImageSourceReader.Read(displayModel.PhotoURL);
                e.Results = new Tuple<MultiModel, BitmapImage>(displayModel, bitmapImage); ;
            }
        }

        private void getDataTaskComplete(object sender, TaskWorkerEventArgs e)
        {
            ProgressBarVisibility = Visibility.Hidden;
            ProgressBarIsEnabled = false;

            if (e.Parameters == null 
                && e.Parameters is int == false &&
                e.Results is Tuple<MultiModel, BitmapImage> == false &&
                e.Cancelled)
            {
                return;
            }

            int index = (int)e.Parameters;
            if (index == SelectedIndex)
            {
                Tuple<MultiModel, BitmapImage> resultData = e.Results as Tuple<MultiModel, BitmapImage>;

                // The individual components need to be changed. If we replace the entire object, the selected index will go to zero.
                PersonTableResults[index].PersonDataTable = resultData.Item1.PersonDataTable;
                PersonTableResults[index].DetailsDataTable = resultData.Item1.DetailsDataTable;
                PersonTableResults[index].AddressDataTable = resultData.Item1.AddressDataTable;

                PersonDataResults = PersonTableResults[index];
                Portrait = resultData.Item2;
            }
        }
    }
}
