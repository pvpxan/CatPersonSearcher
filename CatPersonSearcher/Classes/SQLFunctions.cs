using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatPersonSearcher
{
    // EF6 wrapper.
    public partial class SQLFunctions
    {
        // Do not directly try to call .Dispose() on this. There is a SQLConnection method for that.
        private Context databaseContext = null;

        public SQLFunctions(Context context)
        {
            SetDatabaseContext(context);
        }

        // Likely not needed for this example project, but just in case.
        public void SetDatabaseContext(Context context)
        {
            databaseContext = context;
        }

        public bool AddData(MultiModel multiModel)
        {

            try
            {
                multiModel.PersonDataTable.AddressData = multiModel.AddressDataTable;
                multiModel.PersonDataTable.DetailsData = multiModel.DetailsDataTable;
                databaseContext.PersonData.Add(multiModel.PersonDataTable);
                databaseContext.SaveChanges();
                SQLConnection.UpdateTableCount(1);
                return true;
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error updating data.", Ex);
                return false;
            }
        }

        public bool UpdateData(MultiModel multiModel)
        {
            try
            {
                databaseContext.Entry(multiModel.PersonDataTable).State = EntityState.Modified;
                databaseContext.SaveChanges();
                return true;
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error updating data.", Ex);
                return false;
            }
        }

        public bool UpdatePersonData(PersonData personData)
        {
            try
            {
                databaseContext.Entry(personData).State = EntityState.Modified;
                databaseContext.SaveChanges();
                return true;
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error updating data.", Ex);
                return false;
            }
        }

        public bool DeleteData(MultiModel multiModel)
        {
            try
            {
                // Although my SQL table is set to cascade, I am going to be safe and delete the tables individually.
                databaseContext.Entry(multiModel.AddressDataTable).State = EntityState.Deleted;
                databaseContext.Entry(multiModel.DetailsDataTable).State = EntityState.Deleted;
                databaseContext.SaveChanges();

                databaseContext.Entry(multiModel.PersonDataTable).State = EntityState.Deleted;
                databaseContext.SaveChanges();

                SQLConnection.UpdateTableCount(-1);
                return true;
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error updating data.", Ex);
                return false;
            }
        }
    }
}
