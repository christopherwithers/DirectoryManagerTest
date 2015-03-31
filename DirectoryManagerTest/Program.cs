using System;
using System.Configuration;

namespace DirectoryManagerTest
{
    public class Program
    {
        private readonly static DocumentManagerService _dms = new DocumentManagerService();

        static void Main(string[] args)
        {
            Console.Write(_dms.CreateUploadLocation("Gallery", "Folder2"));
        }
    }
}
