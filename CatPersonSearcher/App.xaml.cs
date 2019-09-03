using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace CatPersonSearcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void AppStartup(object sender, StartupEventArgs e)
        {
            DLLEmbeddingHandler.LoadAssemblies();
            DLLEmbeddingHandler.LoadResourceDictionary("StreamlineMVVM", "Templates/MergedResources.xaml");
            DLLEmbeddingHandler.LoadResourceDictionary("CatPersonSearcher", "Resources/DataTemplates.xaml"); // Loads application DataTemplates AFTER libraries loaded so things work.

            Statics.Initialization();
            WindowFactory.Initialize();
            WindowFactory.OpenWindow(new WindowFactoryData() { WindowType = WindowClassType.MainWindow, Modal = false, });
        }
    }

    // START DLLEmbeddingHandler_Class --------------------------------------------------------------------------------------------------
    public static class DLLEmbeddingHandler
    {
        // NECESSARY for loading embedded resources.
        // ----------------------------------------------------------------------------------------------
        public static void LoadAssemblies()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                return resolve(sender, args);
            };
        }

        private static Assembly resolve(object sender, ResolveEventArgs args)
        {
            string dllName = new AssemblyName(args.Name).Name + ".dll";
            Assembly assembly = Assembly.GetExecutingAssembly();

            string resourceName = assembly.GetManifestResourceNames().FirstOrDefault(rn => rn.EndsWith(dllName));
            if (resourceName == null)
            {
                return null; // Not found, maybe another handler will find it
            }

            System.IO.Stream stream = null;
            Assembly loadedAssembly = null;
            try
            {
                stream = assembly.GetManifestResourceStream(resourceName);
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                loadedAssembly = Assembly.Load(assemblyData);
            }
            catch (Exception Ex)
            {
                loadedAssembly = null;

                MessageBox.Show("Error loading embedded assembly resource. Application will now close." + Environment.NewLine + Convert.ToString(Ex));
                showError("DLL", Ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }

            return loadedAssembly;
        }

        // This is for WPF applications only that reference XAML files built into an assembly.
        public static void LoadResourceDictionary(string assembly, string path)
        {
            // Uri path of assembly resource.
            string uri = @"pack://application:,,,/" + assembly + ";component/" + path;

            // Add Uri to App ResourceDictionary.
            try
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(uri) });
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error loading embedded XAML resource. Application will now close." + Environment.NewLine + Convert.ToString(Ex));
                showError("XAML", Ex);
            }
        }

        private static void showError(string resource, Exception Ex)
        {
            string message = "Error loading embedded " + resource + " resource. Application will now close." + Environment.NewLine + Convert.ToString(Ex);
            MessageBox.Show(message);

            Shutdown();
        }

        public static void Shutdown()
        {

            if (Application.Current != null)
            {
                Application.Current.Shutdown();
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
    // END DLLEmbeddingHandler_Class ----------------------------------------------------------------------------------------------------

    // START Statics_Class --------------------------------------------------------------------------------------------------------------
    public static class Statics
    {
        public static Version CurrentFileVersion { get; } = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
        public static string CurrentUser { get; } = Environment.UserName.ToLower();
        public static string[] Args { get; } = Environment.GetCommandLineArgs();
        public static string ProgramPath { get; } =  getProgramPath();
        public static string DatabaseFile { get; } = getDatabaseFilePath();

        public static string[] ResourceCatalog { get; } = ImageSourceReader.GetResourceNames();

        // Global Settings from config file.
        public static string DefaultSearch { get; set; } = "quick";
        public static bool ShowAll { get; set; } = true;
        public static bool SimulateDelay { get; set; } = false;
        public static int LoadDelay { get; set; } = 0;
        public static int SearchDelay { get; set; } = 0;

        public static void Initialization()
        {
            LogWriter.SetPath(ProgramPath, CurrentUser, "CatPersonSearcher");
            LoadResourceDatabase(false);

            DefaultSearch = Config.Read("default_search");
            if (bool.TryParse(Config.Read("show_all"), out bool showAll) == false)
            {
                ShowAll = showAll;
            }
            if (bool.TryParse(Config.Read("simulate_delay"), out bool simulateDelay))
            {
                SimulateDelay = simulateDelay;
            }
        }

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

        private static string getDatabaseFilePath()
        {
            string file = Config.Read("database_file");
            if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(ProgramPath))
            {
                return "";
            }

            return ProgramPath + @"\Resources\" + file;
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
    }
}
