using System;
using System.Linq;
using MD380FileIO;

namespace MD380Reader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Filename required.");
                Environment.Exit(1);
            }

            Console.WriteLine("loading: {0}", args[0]);
            var file = new MD380File(args[0]);

            if (file.DataLoaded)
            {
                Console.WriteLine("file loaded");
            }
            else
            {
                Environment.Exit(2);
            }

            Console.WriteLine("================");
            Console.WriteLine("Contacts");
            Console.WriteLine("================");
            foreach (var contact in file.Contacts())
            {
                Console.WriteLine("{0}: {1}", contact.Name, contact.DmrID);
            }
            Console.WriteLine();

            Console.WriteLine("================");
            Console.WriteLine("Rx Groups");
            Console.WriteLine("================");
            foreach (var item in file.RxGroups())
            {
                var contacts = file.Contacts().Where(c => item.ContactIDs.Contains(c.ID)).Select(c=>c.Name);
                Console.WriteLine("{0}: {1}", item.Name, string.Join(", ", contacts));
            }
            Console.WriteLine();

            Console.WriteLine("================");
            Console.WriteLine("Zones");
            Console.WriteLine("================");
            foreach (var item in file.Zones())
            {
                var channels = file.Channels().Where(c => item.Channels.Contains(c.ID)).Select(c => c.Name);
                Console.WriteLine("{0}: {1}", item.Name, string.Join(", ", channels));
            }
            Console.WriteLine();

            Console.WriteLine("================");
            Console.WriteLine("Scan Lists");
            Console.WriteLine("================");
            foreach (var item in file.ScanLists())
            {
                var channels = file.Channels().Where(c => item.Channels.Contains(c.ID)).Select(c => c.Name);
                Console.WriteLine("{0}: {1}", item.Name, string.Join(", ", channels));
            }
            Console.WriteLine();


#if DEBUG
            Console.WriteLine("================");
            Console.WriteLine("Press any key to close");
            Console.ReadKey();
#endif
        }
    }
}
