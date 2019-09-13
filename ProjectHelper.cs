using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;

namespace Targets
{
    public static class ProjectHelper
    {
        public static void ChangePropertyGroupCommand(string path, string targets)
        {
            foreach (var file in DirectoryHelper.GetFiles(path, "*.csproj"))
            {
                try
                {
                    var project = new Project(file);

                    foreach (var properties in project.Xml.PropertyGroups)
                    {
                        properties.Parent.RemoveChild(properties);
                        Logger.Info($"Removed {properties.Label}");

                    }
                    project.Xml.AddImport(targets);
                    Logger.Info($"Added ..\\Configurations.targets to {file}");
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
