using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Mono.Options;
using Targets.Commands;

namespace Targets
{
    class Program
    {
        static void Main(string[] args)
        {
            var appName = Assembly.GetCallingAssembly().GetName().Name;
            var folder = string.Empty;
            var imports = new List<string>(); 
            var help = false;
            var deleteImports = false;
            var change = false;
            var commandCount = 0;

            var suite = new OptionSet()
            {
                $"usage: {appName} COMMAND",
                { "h|help", "show options", arg => help = arg != null},
                { "f|folder=", "NECESSARILY: path to folder for recursive .csproj search. ", arg => folder = arg},
                { "c|change",
                    "change all \"PropertyGroup\" to Import Project=\"..\\(name of your targets file)\" " +
                    "with calculated up one level for .csproj files, folder must include *.target file",  arg => change = arg != null},
                { "i|add-import=", "REPEATABLE: add specific import row to all .csproj files",arg => imports.Add(arg)},
                { "d|delete-imports", "delete all imports from .csproj files",arg => deleteImports = arg != null},
            };
            try
            {
                suite.Parse(args);
                if (args.Length == 0)
                    throw new OptionException();
            }
            catch (OptionException e)
            {
                Logger.Error(e.Message);
                Logger.Error("Try `--help' for more information.");
                return;
            }
            if (help)
            {
                ShowHelp(suite);
                return;
            }
            if (!string.IsNullOrEmpty(folder) && !string.IsNullOrEmpty(DirectoryHelper.GetTargetName(folder)) && change)
            {
                ChangeCommand.ChangePropertyGroupCommand(folder);
                commandCount++;
            }
            if (!string.IsNullOrEmpty(folder) && imports.Count > 0)
            {
                AddImportCommand.AddImports(folder, imports);
                commandCount++;
            }
            if (!string.IsNullOrEmpty(folder) && deleteImports)
            {
                DeleteImportsCommand.DeleteImports(folder);
                commandCount++;
            }
            if (commandCount == 0)
            {
                ShowHelp(suite);
                return;
            }
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Targets.exe:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}

