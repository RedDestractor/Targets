using System;
using Microsoft.Build.Evaluation;
using MoreLinq;

namespace Targets.Commands
{
    public static class ChangeCommand
    {
        public static void ChangePropertyGroupCommand(string path, string targets)
        {
            foreach (var file in DirectoryHelper.GetFilesForChange(path, "*.csproj"))
            {
                try
                {
                    Logger.Info($"{file} working...");

                    var project = new Project(file.file);

                    foreach (var property in project.Xml.PropertyGroups)
                    {
                        property.Parent.RemoveChild(property);
                        Logger.Info($"Removed {property.Label}");
                    }

                    project.Xml.AddImport(targets);

                    Logger.Info($"Added Import Project=\"{"\\..".Repeat(file.depth)}\\Configurations.targets");

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
