using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Build.Evaluation;
using MoreLinq.Extensions;
using ReflectionMagic;

namespace Targets
{
    public static class Commands
    {
        public static void AddImportCommand(string path, List<string> imports)
        {
            foreach (var file in DirectoryHelper.GetFiles(path, "*.csproj"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var project = XDocument.Load(file);

                    foreach (var import in imports)
                    {
                        XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
                        project.Root?.AddFirst(new XElement(ns + "Import", new XAttribute("Project", $"{import}")));
                        Logger.Info($"Added Import Project=\"{import}\"");
                    }

                    project.Save(file);
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                }
            }
        }

        public static void AddTargetsCommand(string path)
        {
            foreach (var file in DirectoryHelper.GetFilesForChange(path, "*.csproj"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var project = XDocument.Load(file.file);
                    var targetsName = DirectoryHelper.GetTargetName(path);
                    var import = $"{string.Concat("..\\".Repeat(file.depth))}{targetsName}";

                    XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
                    project.Root?.AddFirst(new XElement(ns + "Import", new XAttribute("Project", $"{import}")));

                    Logger.Info($"Added Import Project=\"{import}");

                    project.Save(file.file);
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                }
            }
        }

        public static void DeleteImportsCommand(string path)
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

        public static void DeletePackagesCommand(string path)
        {
            foreach (var file in DirectoryHelper.GetFiles(path, "packages.config"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var package = XDocument.Load(file);

                    if (package.Root?.Name == "packages")
                    {
                        package.Root.Descendants().Remove();

                        Logger.Info($"Removed package");
                    }

                    package.Save(file);
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                }
            }
        }

        public static void DeletePropertyGroupsCommand(string path)
        {
            foreach (var file in DirectoryHelper.GetFilesForChange(path, "*.csproj"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var project = new Project(file.file);

                    foreach (var property in project.Xml.PropertyGroups)
                    {
                        if (property.Children.Any(p => (string)p.AsDynamic().Name == "OutputType") ||
                            property.Children.Any(p => (string)p.AsDynamic().Name == "RootNamespace") ||
                            property.Children.Any(p => (string)p.AsDynamic().Name == "ProjectGuid") ||
                            property.Children.Any(p => (string)p.AsDynamic().Name == "NuGetPackageImportStamp") ||
                            property.Children.Any(p => (string)p.AsDynamic().Name == "AutoGenerateBindingRedirects") ||
                            property.Children.Any(p => (string)p.AsDynamic().Name == "AssemblyName"))
                        {
                            property.Children
                                .Where(p => (string)p.AsDynamic().Name != "OutputType" &&
                                            (string)p.AsDynamic().Name != "RootNamespace" &&
                                            (string)p.AsDynamic().Name != "ProjectGuid" &&
                                            (string)p.AsDynamic().Name != "NuGetPackageImportStamp" &&
                                            (string)p.AsDynamic().Name != "AutoGenerateBindingRedirects" &&
                                            (string)p.AsDynamic().Name != "AssemblyName")
                                .ForEach(x =>
                                {
                                    property.RemoveChild(x);
                                    Logger.Info($"Removed property group child");
                                });
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

        public static void DeleteReferencesCommand(string path)
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

        public static void DeleteRuntimesCommand(string path)
        {
            foreach (var file in DirectoryHelper.GetFiles(path, "App.config"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var package = XDocument.Load(file);

                    if (package.Root?.Name == "configuration")
                    {
                        var runtime = package.Root.Descendants().FirstOrDefault(x => x.Name == "runtime");

                        runtime?.Remove();

                        Logger.Info($"Removed runtime");
                    }

                    package.Save(file);
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                }
            }
        }

        public static void DeleteTargetsCommand(string path)
        {
            foreach (var file in DirectoryHelper.GetFiles(path, "*.csproj"))
            {
                try
                {
                    Logger.Info($"{file} working...", ConsoleColor.White);

                    var project = new Project(file);

                    foreach (var target in project.Xml.Targets)
                    {
                        target?.Parent?.RemoveChild(target);
                        Logger.Info($"Removed target");
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
