using System;
using System.Collections.Generic;
using Microsoft.Build.Evaluation;

namespace Targets.Commands
{
    public static class AddImportCommand
    {
        public static void AddImports(string path, List<string> imports)
        {
            foreach (var file in DirectoryHelper.GetFiles(path, "*.csproj"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var project = new Project(file);

                    foreach (var import in imports)
                    {
                        project.Xml.AddImport(import);

                        Logger.Info($"Added Import Project=\"{import}\"");
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
