using System;
using Microsoft.Build.Evaluation;
using MoreLinq;

namespace Targets.Commands
{
    public static class ChangeCommand
    {
        public static void ChangePropertyGroupCommand(string path)
        {
            foreach (var file in DirectoryHelper.GetFilesForChange(path, "*.csproj"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var project = new Project(file.file);

                    foreach (var property in project.Xml.PropertyGroups)
                    {
                        property.Parent.RemoveChild(property);
                        Logger.Info($"Removed {property.Label}");
                    }

                    var targetsName = DirectoryHelper.GetTargetName(path);
                    var import = $"{string.Concat("..\\".Repeat(file.depth))}{targetsName}";

                    project.Xml.AddImport(import);

                    Logger.Info($"Added Import Project=\"{import}");

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
