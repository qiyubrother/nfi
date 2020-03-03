using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace nfi
{
    class Program
    {
        static void Main(string[] args)
        {
#if false
            if (args.Length == 0)
            {
                Console.Write("");
                return;
            }
            var fi = new FileInfo(args[0]);
            if (!fi.Exists)
            {
                Console.Write("");
                return;
            }
            var f = @"D:\SVNRoot\src\Client\win\teacherMoveOffice\zntbkt\bin\Debug\zntbkt.exe";
#else
            var f = args[0];
#endif
            var ver = Assembly.LoadFile(f).GetName().Version;
            var v = $"V{ver.Major}.{ver.MajorRevision}.{ver.MinorRevision}";
            Console.Write(v);
        }
    }
}
