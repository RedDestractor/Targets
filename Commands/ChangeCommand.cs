using System;
using System.Linq;
using Microsoft.Build.Evaluation;
using MoreLinq;
using ReflectionMagic;

namespace Targets.Commands
{
    public static class ChangeCommand
    {
        public static void Invoke(string path)
        {
            foreach (var file in DirectoryHelper.GetFilesForChange(path, "*.csproj"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var project = new Project(file.file);

                    foreach (var property in project.Xml.PropertyGroups)
                    {
                        if (property.Children.Count(p => (string)p.AsDynamic().Name == "OutputType" && (string)p.AsDynamic().Name == "OutputType") > 0)
                        {
                            property.Children
                                .Where(p => (string)p.AsDynamic().Name != "OutputType" && (string)p.AsDynamic().Name != "OutputType")
                                .ForEach(x => property.RemoveChild(x));
                        }

                        property.Parent.RemoveChild(property);

                        Logger.Info($"Removed property group");
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
