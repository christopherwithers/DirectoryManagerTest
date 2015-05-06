using System;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;

namespace DirectoryManagerTest
{

    public class Tester
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Program
    {
        private readonly static DocumentManagerService Dms = new DocumentManagerService();
        private static ObjectIDGenerator _objectIdGen = new ObjectIDGenerator();
        private static readonly ICache Cache = new DotNetCache();


        static void Main(string[] args)
        {
           /* var list = new List<Tester> { new Tester { id = 1, name = "sdfdsfddfs" }, new Tester { id = 2, name = "sdfdsfddfs" }, new Tester { id = 3, name = "sdfdsfddfs" }, new Tester { id = 4, name = "sdfdsfddfs" }, new Tester { id = 5, name = "sdfdsfddfs" } };
            var list1 = new[] {1, 2, 3, 4};
            var list2 = new[] {1, 2, 3, 4, 5};
            var list4 = new List<Tester> { new Tester { id = 1, name = "sdfdsfddfs" }, new Tester { id = 2, name = "sdfdsfddfs" }, new Tester { id = 3, name = "sdfdsfddfs" }, new Tester { id = 4, name = "sdfdsfddfs" }, new Tester { id = 5, name = "sdfdsfddfs" } };
            
            bool f;
            bool f1;
            bool f2;
            bool f3;

            var key = _objectIdGen.GetId(list, out f);
            var key1 = _objectIdGen.GetId(list1, out f1);
            var key2 = _objectIdGen.GetId(list2, out f2);
            var key3 = _objectIdGen.GetId(list4, out f3);

            Console.WriteLine("List: {0},{1}", key, f);
            Console.WriteLine("List: {0},{1}", key1, f1);
            Console.WriteLine("List: {0},{1}", key2, f2);
            Console.WriteLine("List: {0},{1}", key3, f3);*/

          /*  var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (var i = 0; i < 100000000; i++)
            {
                var blah = typeof(DocumentManagerService);
              
           
            }

            stopwatch.Stop();

            var ts = stopwatch.Elapsed;
            Console.WriteLine("typeof: {0}ms", ts.TotalMilliseconds);

            stopwatch = new Stopwatch();
            stopwatch.Start();

            for (var i = 0; i < 100000000; i++)
            {
                var blah = Dms.GetType();
            }

            stopwatch.Stop();

            ts = stopwatch.Elapsed;
            Console.WriteLine("gettype: {0}ms", ts.TotalMilliseconds);

            stopwatch = new Stopwatch();
            stopwatch.Start();
            var type = Dms.GetType();
            for (var i = 0; i < 100000000; i++)
            {
                var blah = type;
            }

            stopwatch.Stop();

            ts = stopwatch.Elapsed;
            Console.WriteLine("pre-getType: {0}ms", ts.TotalMilliseconds);*/

            var t1 = new Tester {id = 1, name = "one"};
            var t2 = new Tester {id = 2, name = "two"};

            //Cache.PutItem(t1, t1.id);
          //  Cache.PutItem(t2, new[] { t2.id.ToString(), t2.name });

            Cache.PutItem(t1, t1.id, new CacheDependency { ID = t2.id, Type = t2.GetType() });
            Cache.PutItem(t2, t2.id);

           // Cache.PutItem(t1, new[] { t1.id.ToString(), t1.name });
           // Cache.PutItem(t2, new[] { t2.id.ToString(), t2.name });

          //  t1 = null;
         //   t2 = null;

         //   t1 = Cache.FetchItem<Tester>(1);

           // t1 = Cache.FetchItem<Tester>(1); t1 = Cache.FetchItem<Tester>(1);
           // t1.id = 10;
           // Cache.PutItem(t1, t1.id);
            Cache.Remove(t2, t2.id);
            Console.WriteLine(t1.id);
            //Console.Write(Dms.CreateUploadLocation("Gallery", "Folder3"));
            //   Console.Write(Dms.MergeFolders("Gallery", new []{"New3"}, new [] {"Folder4"}));
        }
    }
}
