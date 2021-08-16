using System.Collections.Concurrent;
using System.IO;

namespace ConsoleApps
{
    public class FileMonitor
    {
        private ConcurrentDictionary<string, FileSystemWatcher> _watchers;

        public FileMonitor()
        {
            _watchers = new ConcurrentDictionary<string, FileSystemWatcher>();
        }

        public void AddWatcher(string name, FileSystemWatcher watcher)
        {
            _watchers.AddOrUpdate(name, watcher, (k, v) => v);
        }

        public void RemoveWatcher(string name)
        {
            if (_watchers.TryGetValue(name, out var watcher))
            {
                watcher.Dispose();
            }
            _watchers.TryRemove(name, out var removeWatcher);
        }
    }
}
