using StreamlineMVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CatPersonSearcher
{
    public static class ImageSourceReader
    {
        public static BitmapImage Read(string image)
        {
            if (string.IsNullOrEmpty(image))
            {
                return null;
            }

            // Checking if the files extension is valid.
            string extenstion = Path.GetExtension(image).ToLower();
            if (extenstion.Contains("jpg") == false &&
                extenstion.Contains("jpeg") == false &&
                extenstion.Contains("bmp") == false &&
                extenstion.Contains("png") == false &&
                extenstion.Contains("tif") == false &&
                extenstion.Contains("tiff") == false &&
                extenstion.Contains("gif") == false)
            {
                return null;
            }

            // Pretty fragile logic check but the best for right now.
            bool resourceFile = image.ToLower().Contains("resourcecats.");
            if (resourceFile == false && File.Exists(image) == false)
            {
                return null;
            }

            BitmapImage bitmapImage = new BitmapImage();
            MemoryStream memoryStream = null;
            try
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

                if (resourceFile)
                {
                    byte[] resourceData = GetEmbeddedResourceAsBytes(image);
                    memoryStream = new MemoryStream(resourceData);
                    bitmapImage.StreamSource = memoryStream;

                }
                else // Must be a local file path.
                {
                    bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bitmapImage.UriSource = new Uri(image, UriKind.RelativeOrAbsolute); // This does not seem to want to work for internal packs.
                }

                // TODO (DB): Clean up my separate image modding class so Rotation can be set if needed.
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error loading image. Path: " + image, Ex);
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }
            }

            return bitmapImage;
        }

        public static byte[] GetEmbeddedResourceAsBytes(string resourceName)
        {
            string resourceAbsoluteName = Statics.ResourceCatalog.FirstOrDefault(rn => rn.ToLower().Contains(resourceName.ToLower())); // Gets resource abosulte name.
            if (string.IsNullOrEmpty(resourceAbsoluteName))
            {
                return null;
            }

            Stream resourceStream = null;
            byte[] imageData = null;
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                resourceStream = assembly.GetManifestResourceStream(resourceAbsoluteName);
                imageData = new byte[resourceStream.Length];
                resourceStream.Read(imageData, 0, imageData.Length);
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error reading image resource.", Ex);
                imageData = null;
            }
            finally
            {
                if (resourceStream != null)
                {
                    resourceStream.Dispose();
                }
            }

            return imageData;
        }

        // Used to get the interal list of Cat images.
        public static string[] GetResourceNames()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceNames();
        }

        // This is silly but seems to work in cleaning up frozen bitmap image byte arrays.
        // At some point I will research this and see what .net versions have this issue.
        public static void Cleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
