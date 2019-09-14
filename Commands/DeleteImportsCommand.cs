using System;
using Microsoft.Build.Evaluation;

namespace Targets.Commands
{
    public class DeleteImportsCommand
    {
        public static void DeleteImports(string path)
        {
            foreach (var file in DirectoryHelper.GetFiles(path, "*.csproj"))
            {
                try
                {
                    Logger.Info($"{file} working...");

                    var project = new Project(file);

                    foreach (var property in project.Xml.Imports)
                    {
                        property.Parent.RemoveChild(property);
                        Logger.Info($"Removed {property.Label}");
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
