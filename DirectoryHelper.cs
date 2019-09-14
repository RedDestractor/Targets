using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Targets
{
    public static class DirectoryHelper
    {
        public static IEnumerable<(string file, int depth)> GetFilesForChange(string path, string pattern)
        {
            var queue = new Queue<string>();

            queue.Enqueue(path);

            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                var depth = 0;

                foreach (var subDir in Directory.GetDirectories(path))
                {
                    queue.Enqueue(subDir);
                    depth = GetDepthForTarget(subDir);
                }

                var files = Directory.GetFiles(path, pattern);

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        yield return (file, depth);
                    }
                }
            }
        }

        public static IEnumerable<string> GetFiles(string path, string pattern)
        {
            var queue = new Queue<string>();

            queue.Enqueue(path);

            while (queue.Count > 0)
            {
                path = queue.Dequeue();

                foreach (var subDir in Directory.GetDirectories(path))
                {
                    queue.Enqueue(subDir);
                }

                var files = Directory.GetFiles(path, pattern);

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        yield return file;
                    }
                }
            }
        }

        public static int GetDepthForTarget(string path)
        {
            var directory = new DirectoryInfo(path ?? Directory.GetCurrentDirectory());
            var depth = 0;

            while (directory != null && !directory.GetFiles("*.targets").Any())
            {
                directory = directory.Parent;
                depth++;
            }

            return --depth;
        }

        public static string GetTargetName(string path)
        {
            var directory = new DirectoryInfo(path ?? Directory.GetCurrentDirectory());
            var name = directory.GetFiles("*.targets", SearchOption.TopDirectoryOnly).FirstOrDefault()?.Name;

            return name;
        }
    }
}
