using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using MoreLinq.Extensions;
using ReflectionMagic;

namespace Targets.Commands
{
    public static class DeletePropertyGroupsCommand
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
                        if (property.Children.Count(p =>
                                (string) p.AsDynamic().Name == "OutputType") > 0)
                        {
                            property.Children
                                .Where(p => (string) p.AsDynamic().Name != "OutputType")
                                .ForEach(x => property.RemoveChild(x));
                        }
                        else
                        {
                            property.Parent.RemoveChild(property);
                        }

                        Logger.Info($"Removed property group");
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
