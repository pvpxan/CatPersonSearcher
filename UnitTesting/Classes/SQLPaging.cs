using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    public class Page
    {
        public int TotalPages { get; set; } = 0;
        public int ActivePage { get; set; } = 0;
        public List<MultiModel> Results { get; set; } = new List<MultiModel>();
    }

    public partial class SQLFunctions
    {
        private readonly int defaultTake = 25; // Number of records per page we want to see.
        private int calculateSkip(int pageNumber)
        {
            return (pageNumber - 1) * defaultTake;
        }
        
        // Returns up to 25 results from the database based on page number.
        public Page GetPageData(int pageNumber, bool fullData, bool hardCount)
        {
            if (pageNumber < 1) // Please pass in a real page number.
            {
                return null;
            }

            int count = SQLConnection.ReadPersonTableCount(hardCount);
            if (count < 1)
            {
                return null;
            }

            Page page = new Page() { ActivePage = pageNumber };
            if (count < (defaultTake + 1)) // No need to run logic when we only have 25 records.
            {
                page.TotalPages = 1;
                page.ActivePage = 1;
                foreach (PersonData personData in databaseContext.PersonData.ToList())
                {
                    page.Results.Add(new MultiModel() { ID = personData.ID, PersonDataTable = personData });
                }
                return page;
            }

            page.TotalPages = ((count - 1) / defaultTake) + 1; // The above checks ensures this does not go badly.
            if (pageNumber >= page.TotalPages)
            {
                page.ActivePage = page.TotalPages;
            }

            if (fullData)
            {
                if (pageNumber == 1)
                {
                    page.Results = GetFullResults(0, defaultTake);
                }
                else
                {
                    page.Results = GetFullResults(calculateSkip(pageNumber), defaultTake);
                }

                return page;
            }

            List<PersonData> pagedPersonData = null;
            if (pageNumber == 1)
            {
                pagedPersonData = GetPersonResults(0, defaultTake);
            }
            else
            {
                pagedPersonData = GetPersonResults(calculateSkip(pageNumber), defaultTake);
            }

            try
            {
                foreach (PersonData personData in pagedPersonData)
                {
                    page.Results.Add(new MultiModel() { ID = personData.ID, PersonDataTable = personData });
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error getting page data. Page Number: " + pageNumber, Ex);
            }

            return page;
        }

        // Runs a search based on a given search string and returns the requested page in for that string.
        // This is not something you would want to use if for massive result sets but it works for right now.
        public Page GetSearchPersonPageData(string search, int pageNumber)
        {
            List<PersonData> results = PersonLikeSearch(search);
            if (results == null || results.Count < 1)
            {
                return null;
            }

            Page page = new Page() { ActivePage = pageNumber };
            if (results.Count < (defaultTake + 1))
            {
                page.TotalPages = 1;
                page.ActivePage = 1;
                foreach (PersonData personData in PersonLikeSearch(search))
                {
                    page.Results.Add(new MultiModel() { ID = personData.ID, PersonDataTable = personData });
                }
                return page;
            }

            page.TotalPages = ((results.Count - 1) / defaultTake) + 1; // The above checks ensures this does not go badly.
            if (pageNumber >= page.TotalPages)
            {
                page.ActivePage = page.TotalPages;
            }

            try
            {
                List<PersonData> pagedPersonData = null;
                if (pageNumber == 1)
                {
                    pagedPersonData = results.Take(defaultTake).ToList();
                }
                else
                {
                    pagedPersonData = results.Skip(calculateSkip(pageNumber)).Take(defaultTake).ToList();
                }

                foreach (PersonData personData in pagedPersonData)
                {
                    page.Results.Add(new MultiModel() { ID = personData.ID, PersonDataTable = personData });
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error getting page data. Page Number: " + pageNumber, Ex);
            }

            return page;
        }

        // Could be made more efficient.
        public Page GetSearchFullDataPage(string search, int pageNumber)
        {
            Page page = GetSearchPersonPageData(search, pageNumber);

            for (int i = 0; i < page.Results.Count; i++)
            {
                int id = (int)page.Results[i].ID; // Should be ok to cast this since I doubt we have more than 2,147,483,647 recods.
                page.Results[i] = GetPersonDetails(id);
            }

            return page;
        }

        // DEPRECATED - Terrible implementation.
        //public Page GetFullDataPage(int pageNumber, bool hardCount)
        //{
        //    Page page = GetPageData(pageNumber, false, hardCount);

        //    for (int i = 0; i < page.Results.Count; i++)
        //    {
        //        int id = (int)page.Results[i].ID; // Should be ok to cast this since I doubt we have more than 2,147,483,647 recods.
        //        page.Results[i] = GetPersonDetails(id);
        //    }

        //    return page;
        //}
    }
}
