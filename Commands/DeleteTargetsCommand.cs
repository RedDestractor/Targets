using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;

namespace Targets.Commands
{
    public static class DeleteTargetsCommand
    {
        public static void Invoke(string path)
        {
            foreach (var file in DirectoryHelper.GetFiles(path, "*.csproj"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var project = new Project(file);

                    foreach (var target in project.Xml.Targets)
                    {
                        target?.Parent?.RemoveChild(target);
                        Logger.Info($"Removed target");
                    }

                    project.Save();
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                }
            }
        }
    }
}
