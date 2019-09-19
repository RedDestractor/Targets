using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Mono.Options;
using Targets.Commands;

namespace Targets
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var appName = Assembly.GetCallingAssembly().GetName().Name;
            var folder = string.Empty;
            var imports = new List<string>(); 
            var help = false;
            var deleteImports = false;
            var deletePropertyGroups = false;
            var deleteReferences = false;
            var deletePackages = false;
            var deleteRuntimes = false;
            var deleteTargets = false;
            var addTargets = false;

            var commandCount = 0;

            var suite = new OptionSet()
            {
                $"usage: {appName} COMMAND",
                { "h|help", "show options", arg => help = arg != null},
                { "f|folder=", "NECESSARILY: path to folder for recursive .csproj search. ", arg => folder = arg},
                { "c|add-targets",
                    "add Import Project=\"..\\(name of your targets file)\" " +
                    "with calculated up one level for .csproj files, folder must contain *.target file",  arg => addTargets = arg != null},
                { "i|add-import=", "REPEATABLE: add specific import row to all .csproj files",arg => imports.Add(arg)},
                { "d|delete-imports", "delete all imports from .csproj files",arg => deleteImports = arg != null},
                { "g|delete-prgroups", "delete all property groups from .csproj files",arg => deletePropertyGroups = arg != null},
                { "r|delete-references", "delete all references from .csproj files",arg => deleteReferences = arg != null},
                { "p|delete-packages", "delete all packages from packages.config files",arg => deletePackages = arg != null},
                { "a|delete-runtimes", "delete all runtimes from App.config files",arg => deleteRuntimes = arg != null},
                { "t|delete-targets", "delete all targets from .csproj files",arg => deleteTargets = arg != null}
            };
            try
            {
                suite.Parse(args);

                if (args.Length == 0)
                {
                    throw new OptionException();
                }
                if (help)
                {
                    ShowHelp(suite);
                    return;
                }
                if (!string.IsNullOrEmpty(folder) && addTargets)
                {
                    if(string.IsNullOrEmpty(DirectoryHelper.GetTargetName(folder)))
                        throw new OptionException("folder must contain *.target file", "change");
                    AddTargetsCommand.Invoke(folder);
                    commandCount++;
                }
                if (!string.IsNullOrEmpty(folder) && imports.Count > 0)
                {
                    AddImportCommand.Invoke(folder, imports);
                    commandCount++;
                }
                if (!string.IsNullOrEmpty(folder) && deleteImports)
                {
                    DeleteImportsCommand.Invoke(folder);
                    commandCount++;
                }
                if (!string.IsNullOrEmpty(folder) && deleteReferences)
                {
                    DeleteReferencesCommand.Invoke(folder);
                    commandCount++;
                }
                if (!string.IsNullOrEmpty(folder) && deletePackages)
                {
                    DeletePackagesCommand.Invoke(folder);
                    commandCount++;
                }
                if (!string.IsNullOrEmpty(folder) && deleteRuntimes)
                {
                    DeleteRuntimesCommand.Invoke(folder);
                    commandCount++;
                }
                if (!string.IsNullOrEmpty(folder) && deleteTargets)
                {
                    DeleteTargetsCommand.Invoke(folder);
                    commandCount++;
                }
                if (!string.IsNullOrEmpty(folder) && deletePropertyGroups)
                {
                    DeletePropertyGroupsCommand.Invoke(folder);
                    commandCount++;
                }
                if (commandCount == 0)
                {
                    throw new OptionException();
                }
            }
            catch (OptionException e)
            {
                Logger.Error(e.Message);
                Logger.Error("Try `--help' for more information.");
            }
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Targets.exe:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}

