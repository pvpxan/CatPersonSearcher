using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CatPersonSearcher
{
    public partial class SearchViewModel : ViewModelBase
    {
        // ViewModel Only Vars
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        TaskWorker populateListViewTask;
        private string currentSearch = "";

        // ViewModel Search Worker
        // ---------------------------------------------------------------------------------------------------------------------------------------------
        private void populateTaskStart(object sender, TaskWorkerEventArgs e)
        {
            if (populateListViewTask.CancellationRequested)
            {
                return;
            }

            if (e.Parameters is Tuple<string, int> == false)
            {
                return;
            }

            Page personPage = null;
            Tuple<string, int> taskData = e.Parameters as Tuple<string, int>;

            simulatedDelay();

            if (populateListViewTask.CancellationRequested)
            {
                return;
            }

            if (string.IsNullOrEmpty(taskData.Item1) || taskData.Item1.Length < 2 || taskData.Item2 < 1)
            {
                personPage = allDataPaginator(taskData.Item2);
            }
            else
            {
                personPage = searchPaginator(taskData.Item1, taskData.Item2);
            }

            e.Results = personPage;
        }

        private Page allDataPaginator(int page)
        {
            SQLFunctions sqlFunctions = new SQLFunctions(SQLConnection.DatabaseContext);

            if (quickSearchParam)
            {
                return sqlFunctions.GetPageData(page, false, false);
            }

            return sqlFunctions.GetPageData(page, true, false);
        }

        private Page searchPaginator(string search, int page)
        {
            SQLFunctions sqlFunctions = new SQLFunctions(SQLConnection.DatabaseContext);

            if (quickSearchParam)
            {
                return sqlFunctions.GetSearchPersonPageData(search, page);
            }

            return sqlFunctions.GetSearchFullDataPage(search, page);
        }

        private void populateTaskComplete(object sender, TaskWorkerEventArgs e)
        {
            ProgressBarVisibility = Visibility.Hidden;
            ProgressBarIsEnabled = false;

            if (e.Cancelled)
            {
                return;
            }

            Page personPage = null;
            if (e.Results != null && e.Results is Page)
            {
                personPage = e.Results as Page;
            }

            PersonTableResults.Clear();
            currentSearch = SearchTextbox;

            if (personPage.Results != null)
            {
                foreach (MultiModel person in personPage.Results)
                {
                    PersonTableResults.Add(person);
                }

                PageCount = personPage.TotalPages;
                PageCurrent = personPage.ActivePage;
            }
        }
    }
}
