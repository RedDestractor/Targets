using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Targets
{
    public static class DirectoryHelper
    {
        public static IEnumerable<string> GetFiles(string path, string pattern)
        {
            var queue = new Queue<string>();

            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (var subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path, pattern);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                }

                if (files != null)
                {
                    foreach (var t in files)
                    {
                        yield return t;
                    }
                }
            }
        }
    }
}
