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
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var project = new Project(file);

                    foreach (var property in project.Xml.Imports)
                    {
                        var projectImport = property.Project;
                        var conditionImport = property.Condition;

                        property.Parent.RemoveChild(property);
                        Logger.Info($"Removed import with project={projectImport} condition={conditionImport}");
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
