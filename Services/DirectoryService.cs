using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LanfeustBridge.Services
{
    public class DirectoryService
    {
        private static Lazy<DirectoryService> _service = new Lazy<DirectoryService>(() => new DirectoryService());

        public static DirectoryService Service { get { return _service.Value; } }

        public string LogDirectory { get; private set; }
        public string DataDirectory { get; private set; }

        public DirectoryService()
        {
            string currentDir = Environment.CurrentDirectory;
            if (currentDir.StartsWith(@"D:\home\site", StringComparison.InvariantCultureIgnoreCase))
            {
                // in Azure
                LogDirectory = @"D:\home\LogFiles";
                DataDirectory = @"D:\home\data";
            }
            else
            {
                LogDirectory = Path.Combine(currentDir, "logs");
                if (!Directory.Exists(LogDirectory))
                    Directory.CreateDirectory(LogDirectory);
                DataDirectory = Path.Combine(currentDir, "data");
                if (!Directory.Exists(DataDirectory))
                    Directory.CreateDirectory(DataDirectory);
            }
        }
    }
}
