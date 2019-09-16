using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Build.Evaluation;

namespace Targets.Commands
{
    public static class DeletePackagesCommand
    {
        public static void Invoke(string path)
        {
            foreach (var file in DirectoryHelper.GetFiles(path, "packages.config"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var package = XDocument.Load(file);

                    if (package.Root?.Name == "packages")
                    {
                        package.Root.Descendants().Remove();
                    }

                    package.Save(file);
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                }
            }
        }
    }
}
