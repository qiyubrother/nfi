using System;
using System.IO;
using System.Reflection;

namespace nfi
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]) || IsHelpOption(args[0]))
            {
                PrintUsage();
                // 对于无参数或仅查看帮助的情况，返回 0 更友好
                return;
            }

            var path = args[0];

            if (!File.Exists(path))
            {
                Console.Error.WriteLine($"错误：找不到文件：\"{path}\"");
                Environment.ExitCode = 1;
                return;
            }

            try
            {
                // 使用 AssemblyName.GetAssemblyName 只读取元数据，不真正加载到当前进程域
                var assemblyName = AssemblyName.GetAssemblyName(path);
                var ver = assemblyName.Version;
                if (ver == null)
                {
                    Console.Error.WriteLine("错误：无法读取程序集版本信息。");
                    Environment.ExitCode = 1;
                    return;
                }

                // 原来使用 MajorRevision 是不合理的，这里修正为常见的三段式版本：主.次.构建
                var v = $"V{ver.Major}.{ver.Minor}.{ver.Build}";
                Console.Write(v);
            }
            catch (BadImageFormatException)
            {
                Console.Error.WriteLine($"错误：目标文件不是有效的 .NET 程序集：\"{path}\"");
                Environment.ExitCode = 1;
            }
            catch (FileLoadException ex)
            {
                Console.Error.WriteLine($"错误：加载程序集失败：{ex.Message}");
                Environment.ExitCode = 1;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("发生未预期的错误：");
                Console.Error.WriteLine(ex.Message);
                Environment.ExitCode = 1;
            }
        }

        private static bool IsHelpOption(string arg)
        {
            var value = arg.Trim();
            return string.Equals(value, "-h", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(value, "--help", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(value, "/h", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(value, "/?", StringComparison.OrdinalIgnoreCase);
        }

        private static void PrintUsage()
        {
            Console.WriteLine("用法：");
            Console.WriteLine("  nfi <程序集路径>");
            Console.WriteLine();
            Console.WriteLine("说明：");
            Console.WriteLine("  读取指定 .NET 程序集的版本号，并输出为形如 \"V主.次.构建\" 的格式。");
            Console.WriteLine();
            Console.WriteLine("示例：");
            Console.WriteLine("  nfi C:\\path\\to\\your\\app.exe");
        }
    }
}
