using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatPersonSearcher
{
    // EF6 wrapper.
    public partial class SQLFunctions
    {
        // Gets a list of people by just searching the person table.
        // Be cause of large data sets with potentially lots of results.
        public List<PersonData> PersonLikeSearch(string search)
        {
            if (databaseContext == null || search.Length < 2)
            {
                return null;
            }

            try
            {
                // So there is no "LIKE" option in this SQLite EF6 nuget. Oh well.
                return databaseContext.PersonData.SqlQuery(
                    "SELECT * FROM PersonData WHERE PersonData.Firstname LIKE '%" + search + "%' OR PersonData.Lastname LIKE '%" + search + "%'").ToList();
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error running QueryPersonResultsQuick request. Pattern: " + search, Ex);
                return null;
            }
        }

        // Uses ID to find data for a person from the Address and Details tables.
        public MultiModel GetPersonDetails(long id)
        {
            if (databaseContext == null || id < 0)
            {
                return null;
            }

            List<MultiModel> records = null;
            try
            {
                records =
                    (from person in databaseContext.PersonData
                     join address in databaseContext.AddressData on person.ID equals address.ID
                     join details in databaseContext.DetailsData on person.ID equals details.ID
                     where person.ID == id
                     select new MultiModel()
                     {
                         ID = person.ID,
                         PersonDataTable = person,
                         AddressDataTable = address,
                         DetailsDataTable = details,
                     }).ToList();
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error running GetPersonDetails request. Search ID: " + Convert.ToString(id), Ex);
                return null;
            }

            if (records == null || records.Count() < 1 || records[0] == null) // Bad or empty data.
            {
                return null;
            }

            return records[0];
        }

        // The 2 functions below should be used with care for large data sets with large takes.
        // Never ended up using this.
        public List<MultiModel> GetFullResults(int skip, int take)
        {
            try
            {
                if (skip == 0)
                {
                    return
                        (from person in databaseContext.PersonData
                         join address in databaseContext.AddressData on person.ID equals address.ID
                         join details in databaseContext.DetailsData on person.ID equals details.ID
                         select new MultiModel()
                         {
                             ID = person.ID,
                             PersonDataTable = person,
                             AddressDataTable = address,
                             DetailsDataTable = details,
                         }).Take(take).ToList();
                }
                else
                {
                    return
                        (from person in databaseContext.PersonData
                         join address in databaseContext.AddressData on person.ID equals address.ID
                         join details in databaseContext.DetailsData on person.ID equals details.ID
                         select new MultiModel()
                         {
                             ID = person.ID,
                             PersonDataTable = person,
                             AddressDataTable = address,
                             DetailsDataTable = details,
                         }).OrderBy(p => p.ID).Skip(skip).Take(take).ToList();
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error getting full result set. " +
                    "Skip: " + Convert.ToString(skip) + "Take: " + Convert.ToString(take), Ex);
                return null;
            }
        }

        public List<PersonData> GetPersonResults(int skip, int take)
        {
            try
            {
                //string taken = Convert.ToString(take);

                if (skip == 0)
                {
                    return (from person in databaseContext.PersonData
                            select person).Take(take).ToList();

                    // Manual query if I wanted to.
                    //pagedPersonData = databaseContext.PersonData.SqlQuery
                    //    ("Select * FROM PersonData LIMIT " + taken).ToList();
                }
                else
                {
                    return
                        (from person in databaseContext.PersonData
                         select person).OrderBy(p => p.ID).Skip(skip).Take(take).ToList();

                    // Manual query if I wanted to.
                    //pagedPersonData = databaseContext.PersonData.SqlQuery
                    //    ("Select * FROM PersonData LIMIT " + taken + " OFFSET " + Convert.ToString(skip(pageNumber))).ToList();
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error getting PersonData result set. " +
                    "Skip: " + Convert.ToString(skip) + "Take: " + Convert.ToString(take), Ex);
                return null;
            }
        }
    }
}
