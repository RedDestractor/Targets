using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Targets.Commands
{
    public static class DeleteRuntimesCommand
    {
        public static void Invoke(string path)
        {
            foreach (var file in DirectoryHelper.GetFiles(path, "App.config"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var package = XDocument.Load(file);

                    if (package.Root?.Name == "configuration")
                    {
                        var runtime = package.Root.Descendants().FirstOrDefault(x => x.Name == "runtime");

                        runtime?.Remove();

                        Logger.Info($"Removed runtime");
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
