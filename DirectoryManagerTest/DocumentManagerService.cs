using System;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Web.Hosting;
using eMotive.CMS.Extensions;

namespace DirectoryManagerTest
{
    public class DocumentManagerService
    {
        private readonly string _uploadDirectory;

        public DocumentManagerService()
        {
            _uploadDirectory = ConfigurationManager.AppSettings["Uploads"]; ;
        }


        public bool CreateUploadLocation(string application, params string[] location)
        {
            //todo: check if location empty? if so, just use application??

            var dirLocation = string.Join("/", location);
            var path = string.Format("{0}/{1}/{2}", _uploadDirectory, application, dirLocation);

            if (Directory.Exists(path))
            {
                Console.WriteLine("Dir Exists");
                return false;
            }

            try
            {

                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {//if we can't create the gallery folder, we delete the gallery from the DB and remove it from search.
                Console.WriteLine("Error: {0}", ex);
                return false;
            }
            return true;
        }

        public bool ChangeUploadLocation(string application, string oldLocation, string newLocation)
        {
            return ChangeUploadLocation(application, new [] {oldLocation}, new [] {newLocation});
        }

        public bool ChangeUploadLocation(string application, string[] oldLocation, string[] newLocation)
        {
            var oldDirLocation = string.Join("/", oldLocation);
            var newDirLocation = string.Join("/", newLocation);
            var oldPath = string.Format("{0}/{1}/{2}", _uploadDirectory, application, oldDirLocation);
            var newPath = string.Format("{0}/{1}/{2}", _uploadDirectory, application, newDirLocation);

            if (!Directory.Exists(oldPath))
            {
                Console.WriteLine("Old Dir Not Found!");
                return false;
            }

            if (Directory.Exists(newPath))
            {
                Console.WriteLine("New DIr Already Exists!");
                return false;
            }

            try
            {
                Directory.Move(oldPath, newPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
                return false;
            }

            return true;
        }

        public bool ChangeUploadApplication(string oldApplication, string newApplication)
        {
            var oldPath = string.Format("{0}/{1}/{2}", _uploadDirectory, oldApplication);
            var newPath = string.Format("{0}/{1}/{2}", _uploadDirectory, newApplication);

            if (!Directory.Exists(oldPath))
            {
                Console.WriteLine("Old Dir Not Found!");
                return false;
            }

            if (Directory.Exists(newPath))
            {
                Console.WriteLine("New DIr Already Exists!");
                return false;
            }

            try
            {
                Directory.Move(oldPath, newPath);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static void Delete(string toDelete)
        {
            var downloadedMessageInfo = new DirectoryInfo(toDelete);

            foreach (var file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (var dir in downloadedMessageInfo.GetDirectories())
            {
                dir.Delete(true);
            }

            Directory.Delete(toDelete);
        }

        public bool MergeFolders(string application, string[] oldLocation, string[] newLocation)
        {
            var oldDirLocation = string.Join("/", oldLocation);
            var newDirLocation = string.Join("/", newLocation);
            var oldPath = string.Format("{0}/{1}/{2}", _uploadDirectory, application, oldDirLocation);
            var newPath = string.Format("{0}/{1}/{2}", _uploadDirectory, application, newDirLocation);

            if (!Directory.Exists(oldPath))
            {
                Console.WriteLine("Old Dir Not Found!");
                return false;
            }

            if (!Directory.Exists(newPath))
            {
                Console.WriteLine("New Dir Not Found!");
                return false;
            }
            var success = true;
            try
            {

                CopyAll(new DirectoryInfo(oldPath), new DirectoryInfo(newPath));
            }
            catch (Exception ex)
            {
                success = false;
            }

            if(success)
                Delete(newPath);
            return success;
        }

        public bool MergeUploadWithApplication(string oldApplication, string applicationToMerge)
        {
            if (String.Equals(oldApplication, applicationToMerge, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            var oldApp = string.Format("{0}/{1}", _uploadDirectory, oldApplication);
            var appToMerge = string.Format("{0}/{1}", _uploadDirectory, applicationToMerge);

            // Check if the target directory exists, if not, create it.
            if (!Directory.Exists(oldApp))
            {
                //Directory.CreateDirectory(target.FullName);
                return false;
            }

            if (!Directory.Exists(applicationToMerge))
            {
                //Directory.CreateDirectory(target.FullName);
                return false;
            }

            CopyAll(new DirectoryInfo(appToMerge), new DirectoryInfo(oldApp));

            return true;
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Copy each file into it's new directory.
            foreach (var fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (var diSourceSubDir in source.GetDirectories())
            {
                var nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
