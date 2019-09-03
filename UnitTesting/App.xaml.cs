using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace UnitTesting
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void AppStartup(object sender, StartupEventArgs e)
        {
            Statics.Initialization();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }

    // START Statics_Class --------------------------------------------------------------------------------------------------------------
    public static class Statics
    {
        public static void Initialization()
        {
            LogWriter.SetPath(ProgramPath, "TestingUser", "UnitTesting");
            LoadResourceDatabase(false);
        }

        // This is only necessary for this assessment as a means of getting the database initially loaded up.
        public static void LoadResourceDatabase(bool overwrite)
        {
            byte[] db = Properties.Resources.PersonData;

            if (System.IO.File.Exists(DatabaseFile) && overwrite == false)
            {
                return;
            }

            try
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(DatabaseFile));
                System.IO.File.WriteAllBytes(DatabaseFile, db);
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error loading database file.", Ex);
            }
        }

        public static string ProgramPath { get; } = getProgramPath();
        private static string getProgramPath()
        {
            try
            {
                if (AppDomain.CurrentDomain.BaseDirectory[AppDomain.CurrentDomain.BaseDirectory.Length - 1] == '\\')
                {
                    return AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
                }
                else
                {
                    return Environment.CurrentDirectory;
                }
            }
            catch
            {
                // Doubt this will ever happen.
                return "";
            }
        }

        public static string DatabaseFile { get; } = getDatabaseFilePath();
        private static string getDatabaseFilePath()
        {
            return ProgramPath + @"\resources\PersonData.db";
        }
    }
}
