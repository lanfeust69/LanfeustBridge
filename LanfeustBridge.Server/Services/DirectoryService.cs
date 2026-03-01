namespace LanfeustBridge.Services;

public class DirectoryService
{
    public DirectoryService()
    {
        string currentDir = Directory.GetCurrentDirectory();
        if (currentDir.StartsWith(@"C:\home\site", StringComparison.OrdinalIgnoreCase))
        {
            // in Azure
            LogDirectory = @"C:\home\LogFiles";
            DataDirectory = @"C:\home\data";
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
