using System;
using System.Linq;
using Microsoft.Build.Evaluation;
using MoreLinq;
using ReflectionMagic;

namespace Targets.Commands
{
    public static class AddTargetsCommand
    {
        public static void Invoke(string path)
        {
            foreach (var file in DirectoryHelper.GetFilesForChange(path, "*.csproj"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var project = new Project(file.file);
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
