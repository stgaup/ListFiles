using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ListFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                ShowHelp();
                return;
            }

            var rootPath = args[0];
            var fileName = args[1];

            if(args.Length < 3)
                ListFiles(rootPath, fileName);

            string publicKeyToken = null;
            string version = null;

            var a = 2;

            while (a < args.Length)
            {
                var s = args[a].Substring(0, 3);
                switch (s)
                {
                    case "-v:":
                        version = s.Substring(3);
                        break;
                    case "-p:":
                        publicKeyToken = s.Substring(3);
                        break;
                }
                a++;
            }

            ListFiles(rootPath, fileName, publicKeyToken, version);
            Console.ReadLine();
        }

        static void ListFiles(string rootPath, string fileName, string publicKeyToken = null, string version = null)
        {
            var curDir = new DirectoryInfo(rootPath);

            FileInfo[] files;
            try
            {
                files = curDir.GetFiles();
            }
            catch (Exception e)
            {
                return;
            }


            //list files
            foreach (var f in files)
            {
                if (f.Name == fileName)
                {
                    var asm = Assembly.LoadFrom(f.FullName);
                    if (!string.IsNullOrWhiteSpace(publicKeyToken) && !string.IsNullOrWhiteSpace(version))
                    {
                        if(asm.FullName.Contains(publicKeyToken) && asm.FullName.Contains(version))
                            Console.WriteLine($"{f.LastWriteTime} ## {asm.FullName} ## {f.FullName}");
                        return;
                    }
                    if (!string.IsNullOrWhiteSpace(publicKeyToken))
                    {
                        if(asm.FullName.Contains(publicKeyToken))
                            Console.WriteLine($"{f.LastWriteTime} ## {asm.FullName} ## {f.FullName}");
                        return;
                    }
                    if (!string.IsNullOrWhiteSpace(version))
                    {
                        if(asm.FullName.Contains(version))
                            Console.WriteLine($"{f.LastWriteTime} ## {asm.FullName} ## {f.FullName}");
                        return;
                    }

                    Console.WriteLine($"{f.LastWriteTime} ## {asm.FullName} ## {f.FullName}");

                }
            }

            var subs = curDir.GetDirectories();
            foreach (var d in subs)
            {
                ListFiles(d.FullName, fileName, publicKeyToken);
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("ListFiles v. 0.1");
            Console.WriteLine("  Utility that searches for files by name, listing all found with FullName that contains the PublicKeyToken and version.");
            Console.WriteLine("  Also possible to filter on publicKeyToken and/or version.");
            Console.WriteLine("Usage:");
            Console.WriteLine("  ListFiles [rootDirectory] [fileName] [-p:publicKeyToken] [-v:version]");
            Console.WriteLine("Example:");
            Console.WriteLine("  ListFiles C:\\projects log4net.dll -p:692fbea5521e1304 -v:1.2.10.0");
            Console.WriteLine();
        }
    }
}
