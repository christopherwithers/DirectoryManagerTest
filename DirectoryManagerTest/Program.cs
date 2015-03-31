using System;
using System.Configuration;

namespace DirectoryManagerTest
{
    public class Program
    {
        private readonly static DocumentManagerService Dms = new DocumentManagerService();

        static void Main(string[] args)
        {
            //Console.Write(Dms.CreateUploadLocation("Gallery", "Folder3"));
            Console.Write(Dms.MergeFolders("Gallery", new []{"New3"}, new [] {"Folder4"}));
        }
    }
}
