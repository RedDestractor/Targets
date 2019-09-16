using Microsoft.Build.Evaluation;
using System;
using System.Linq;

namespace Targets.Commands
{
    public static class DeleteReferencesCommand
    {
        public static void Invoke(string path)
        {
            foreach (var file in DirectoryHelper.GetFiles(path, "*.csproj"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var project = new Project(file);

                    foreach (var itemGroup in project.Xml.ItemGroups)
                    {
                        foreach (var property in itemGroup.Children.Where(p => p.GetType().GetProperty("ItemType")?.GetValue(p).ToString() == "Reference"))
                        {
                            itemGroup.RemoveChild(property);
                            Logger.Info($"Removed reference");
                        }
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
