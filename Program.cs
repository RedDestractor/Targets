using System;
using System.Linq;
using System.Reflection;
using Mono.Options;

namespace Targets
{
    class Program
    {
        static void Main(string[] args)
        {
            var appName = Assembly.GetCallingAssembly().GetName().Name;
            var folder = string.Empty;
            var targets = string.Empty;
            var isHelpNeeded = false;
            
            var suite = new OptionSet()
            {
                $"usage: {appName} COMMAND",
                { "h|help", "show options", h => isHelpNeeded = true},
                { "f|folder=", "path to folder to change property groups of projects in this folder", arg => folder = arg},
                { "t|targets=", "path to target file (value for Imports=\"...\")",arg => targets = arg},
            };
            try
            {
                suite.Parse(args);

                if (args.Length == 0)
                    throw new OptionException();
            }
            catch (OptionException e)
            {
                // output some error message
                Logger.Error(e.Message);
                Logger.Error("Try `--help' for more information.");
                return;
            }
            if (isHelpNeeded)
            {
                ShowHelp(suite);
                return;
            }

            ProjectHelper.ChangePropertyGroupCommand(folder, targets);
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}

