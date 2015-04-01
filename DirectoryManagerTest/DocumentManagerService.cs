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

        public bool MergeFoldersWithinApplication(string application, string[] mergeTo, string[] mergeFrom)
        {
            var mergeFromLocation = string.Join("/", mergeFrom);
            var mergeToLocation = string.Join("/", mergeTo);

            var mergeFromPath = string.Format("{0}/{1}/{2}", _uploadDirectory, application, mergeFromLocation);
            var mergeToPath = string.Format("{0}/{1}/{2}", _uploadDirectory, application, mergeToLocation);

            if (!Directory.Exists(mergeFromPath))
            {
                Console.WriteLine("Merge From Directory Not Found!");
                return false;
            }

            if (!Directory.Exists(mergeToPath))
            {
                Console.WriteLine("Merge From Directory Not Found!");
                return false;
            }

            var success = true;

            try
            {

                CopyAll(new DirectoryInfo(mergeFromPath), new DirectoryInfo(mergeToPath));
            }
            catch (Exception ex)
            {
                success = false;
            }

            if(success)
                Delete(mergeFromPath);

            return success;
        }

        public bool MergeApplicationFolders(string mergeToApplication, string mergeFromApplication)
        {
          /*  if (String.Equals(mergeToApplication, mergeFromApplication, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            var mergeToPath = string.Format("{0}/{1}", _uploadDirectory, mergeToApplication);
            var mergeFromPath = string.Format("{0}/{1}", _uploadDirectory, mergeFromApplication);

            if (!Directory.Exists(mergeToPath))
            {
                Console.WriteLine("Merge To Directory Not Found!");
                return false;
            }

            if (!Directory.Exists(mergeFromPath))
            {
                Console.WriteLine("Merge From Directory Not Found!");
                return false;
            }

            var success = true;

            try
            {
                CopyAll(new DirectoryInfo(mergeFromPath), new DirectoryInfo(mergeToPath));
            }
            catch (Exception ex)
            {
                success = false;
            }

            if (success)
                Delete(mergeFromPath);

            return success;*/
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (var fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            foreach (var diSourceSubDir in source.GetDirectories())
            {
                var nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
