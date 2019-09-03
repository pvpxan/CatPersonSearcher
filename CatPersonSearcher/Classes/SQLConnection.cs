using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatPersonSearcher
{
    // Creates an SQL connection.
    public static class SQLConnection
    {
        // Do not directly try to call .Dispose() on this. There is a SQLConnection method for that.
        public static Context DatabaseContext = null;

        // Saves a connection to the static DatabaseContext object.
        public static bool Initialize(string path)
        {
            if (System.IO.File.Exists(path) == false)
            {
                return false;
            }

            // The database is being reinitialized.
            if (DatabaseContext != null)
            {
                DatabaseContext.Dispose();
            }

            int timeout;
            if (int.TryParse(Config.Read("database_timeout"), out timeout) == false)
            {
                timeout = 60;
            }

            SQLiteConnection connection = connect(path, timeout);
            if (connection == null)
            {
                return false;
            }

            try
            {
                DatabaseContext = new Context(connection, timeout);
                ValidateCount();
                return true;
            }
            catch (Exception Ex)
            {
                LogWriterExtended.ExceptionDisplay("Error making SQLite connection.", Ex, true);
                return false;
            }
        }

        // Actually performs the connections.
        private static SQLiteConnection connect(string path, int timeout)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            try
            {
                return new SQLiteConnection()
                {
                    ConnectionString = new SQLiteConnectionStringBuilder() { DataSource = path, ForeignKeys = true, DefaultTimeout = timeout }.ConnectionString
                };
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error creating SQLiteConnection connection string.", Ex);
                return null;
            }
        }

        // Closes the DatabaseContext object.
        public static void Close()
        {
            if (DatabaseContext != null)
            {
                DatabaseContext.Dispose();
                DatabaseContext = null;
            }
        }

        // Assuming anyone adding data is properly updating this table, the count should be accurate.
        public static int ReadPersonTableCount(bool hardCount)
        {
            if (DatabaseContext == null)
            {
                return -1;
            }

            if (hardCount)
            {
                try
                {
                    return DatabaseContext.PersonData.Count();
                }
                catch (Exception Ex)
                {
                    LogWriter.Exception("Something went wrong running count on Table Count table", Ex);
                    return -1;
                }
            }

            List<TableCount> data = readTableCountTable();
            if (data == null)
            {
                try
                {
                    return DatabaseContext.PersonData.Count();
                }
                catch (Exception Ex)
                {
                    LogWriter.Exception("Error reading table count data.", Ex);
                    return -1;
                }
            }

            return readPersonTableCount(data);
        }

        private static List<TableCount> readTableCountTable()
        {
            try
            {
                List<TableCount> tableCountList =
                    (from tableCountData in DatabaseContext.TableCount
                     select tableCountData).ToList();

                if (tableCountList == null)
                {
                    LogWriter.LogEntry("Null data for TableCount of PersonData. This should not happen and your database is probably all sorts of jacked up.");
                    return null;
                }

                return tableCountList;
            }
            catch (Exception Ex)
            {
                LogWriterExtended.ExceptionDisplay("Error get count list data.", Ex, true);
                return null;
            }
        }

        private static int readPersonTableCount(List<TableCount> data)
        {
            if (data.Count < 1 || data[0] == null)
            {
                ValidateCount();
            }

            try
            {
                int tableCount = (int)data[0].PersonCount;
                return tableCount;
            }
            catch (Exception Ex)
            {
                LogWriterExtended.ExceptionDisplay("Error reading count data from list record.", Ex, true);
                return -1;
            }
        }

        public static bool UpdateTableCount(int alter)
        {
            List<TableCount> data = readTableCountTable();
            if (data == null) // Database schema error. Cannot update.
            {
                return false;
            }

            if (data.Count < 1 || data[0] == null) // Table exists but has bad data. Rewrite it all.
            {
                ValidateCount();
                return true;
            }

            int count = readPersonTableCount(data);
            data[0].PersonCount = count + alter;
            writePersonTableCount(data);
            return true;
        }

        // Verifies the count data.
        public static void ValidateCount()
        {
            List<TableCount> data = readTableCountTable();
            if (data == null)
            {
                // Nothing we can do as the database is likely jacked up.
                return;
            }

            try
            {
                int hardCount = DatabaseContext.PersonData.Count();
                if (data.Count < 1)
                {
                    data.Add(new TableCount() { PersonCount = hardCount });
                    writePersonTableCount(data);
                    return;
                }

                if (data[0] == null)
                {
                    data[0] = new TableCount() { PersonCount = hardCount };
                    writePersonTableCount(data);
                    return;
                }

                if (readPersonTableCount(data) != hardCount)
                {
                    data[0].PersonCount = hardCount;
                    writePersonTableCount(data);
                    return;
                }
            }
            catch (Exception Ex)
            {
                LogWriterExtended.ExceptionDisplay("Error running count update.", Ex, true);
            }
        }

        private static void writePersonTableCount(List<TableCount> data)
        {
            try
            {
                DatabaseContext.Entry(data[0]).State = EntityState.Modified;
                DatabaseContext.SaveChanges();
            }
            catch (Exception Ex)
            {
                LogWriterExtended.ExceptionDisplay("Error writing to database. What did you do?", Ex, true);
            }
        }
    }

    // This class is not intended to be accessed outside of the SQLConnection class. Yes...this is sloppy.
    public class Context : DbContext
    {
        public Context(SQLiteConnection connection, int timeout) : base(connection, true)
        {
            this.Database.CommandTimeout = timeout;
        }

        public virtual DbSet<PersonData> PersonData { get; set; }
        public virtual DbSet<AddressData> AddressData { get; set; }
        public virtual DbSet<DetailsData> DetailsData { get; set; }
        public virtual DbSet<TableCount> TableCount { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Person
            modelBuilder.Entity<PersonData>()
                .Property(e => e.Firstname)
                .IsUnicode(false);

            modelBuilder.Entity<PersonData>()
                .Property(e => e.Lastname)
                .IsUnicode(false);

            modelBuilder.Entity<PersonData>()
                .HasOptional(e => e.AddressData)
                .WithRequired(e => e.PersonData);

            modelBuilder.Entity<PersonData>()
                .HasOptional(e => e.DetailsData)
                .WithRequired(e => e.PersonData);

            // Details
            modelBuilder.Entity<DetailsData>()
                .Property(e => e.Age)
                .IsUnicode(false);

            modelBuilder.Entity<DetailsData>()
                .Property(e => e.Interests)
                .IsUnicode(false);

            modelBuilder.Entity<DetailsData>()
                .Property(e => e.PhotoURL)
                .IsUnicode(false);

            // Address
            modelBuilder.Entity<AddressData>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<AddressData>()
                .Property(e => e.Street)
                .IsUnicode(false);

            modelBuilder.Entity<AddressData>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<AddressData>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<AddressData>()
                .Property(e => e.Zipcode)
                .IsUnicode(false);
        }
    }
}
