using System;
using System.Configuration;
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
            throw new System.NotImplementedException();
        }

        public bool ChangeUploadLocation(string application, string[] oldLocation, string[] newLocation)
        {
            var oldDirLocation = string.Join("/", oldLocation);
            var newDirLocation = string.Join("/", newLocation);
            var path = string.Format("{0}/{1}/{2}", _uploadDirectory, application, dirLocation);

            if (Directory.Exists(path))
            {
                Console.WriteLine("Dir Exists");
                return false;
            }
        }

        public bool ChangeUploadApplication(string oldApplication, string newApplication)
        {
            throw new System.NotImplementedException();
        }

        public bool MergeUploadWithApplication(string oldApplication, string applicationToMerge)
        {
            throw new System.NotImplementedException();
        }
    }
}
