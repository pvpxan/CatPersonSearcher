using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    public static class TestOutput
    {
        // This is all just to help me see if I can rebuild a database if I need to from an existing one.

        public static void OutputRecordInserts()
        {
            Context databaseContext = SQLConnection.DatabaseContext;

            List<MultiModel> records = null;
            try
            {
                records =
                    (from person in databaseContext.PersonData
                     join address in databaseContext.AddressData on person.ID equals address.ID
                     join details in databaseContext.DetailsData on person.ID equals details.ID
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
                LogWriter.Exception("Error running get all query.", Ex);
            }

            List<string> insertPersonList = new List<string>();
            List<string> insertAddressList = new List<string>();
            List<string> insertDetailsList = new List<string>();
            foreach (MultiModel item in records)
            {
                string insertPersonData = "INSERT INTO `PersonData` (`Firstname`,`Lastname`) VALUES (\"" + item.Firstname + "\",\"" + item.Lastname + "\");";
                insertPersonList.Add(insertPersonData);

                string insertAddressData = "INSERT INTO `AddressData` (`ID`,`Phone`,`Street`,`City`,`State`,`Zipcode`) VALUES (\"" + item.ID + "\",\"" + item.Phone + "\",\"" + item.Street + "\",\"" + item.City + "\",\"" + item.State + "\",\"" + item.Zipcode + "\");";
                insertAddressList.Add(insertAddressData);

                string insertDetailsData = "INSERT INTO `DetailsData` (`ID`,`Age`,`Interests`,`PhotoURL`) VALUES (\"" + item.ID + "\",\"" + item.Age + "\",\"" + item.Interests + "\",\"\");";
                insertDetailsList.Add(insertDetailsData);
            }

            System.IO.File.WriteAllLines(Statics.ProgramPath + @"\SQLite_InsertPersonData.txt", insertPersonList.ToArray());
            System.IO.File.WriteAllLines(Statics.ProgramPath + @"\SQLite_InsertAddressData.txt", insertAddressList.ToArray());
            System.IO.File.WriteAllLines(Statics.ProgramPath + @"\SQLite_InsertDetailsData.txt", insertDetailsList.ToArray());
        }

        public static void OutputResourceName()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            List<string> insertQuery = new List<string>();
            foreach (string file in assembly.GetManifestResourceNames())
            {
                if (file.Contains("ResourceCats"))
                {
                    string[] nameSplit = file.Split('.');

                    insertQuery.Add("UPDATE DetailsData SET PhotoURL = " +
                        "\"ResourceCats." + nameSplit[2] + ".jpg\" WHERE ID = " + Convert.ToString(insertQuery.Count + 1) + ";");
                }
            }

            System.IO.File.WriteAllLines(Statics.ProgramPath + @"\SQLite_UpdatePhotoURL.txt", insertQuery.ToArray());
        }
    }
}
