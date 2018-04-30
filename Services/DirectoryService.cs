using System;
using System.IO;

namespace LanfeustBridge.Services
{
    public class DirectoryService
    {
        public DirectoryService()
        {
            string currentDir = Directory.GetCurrentDirectory();
            if (currentDir.StartsWith(@"D:\home\site", StringComparison.OrdinalIgnoreCase))
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

        public string LogDirectory { get; private set; }

        public string DataDirectory { get; private set; }
    }
}
