namespace LanfeustBridge.Services;

public class DbService
{
    public DbService(ILogger<DbService> logger, DirectoryService directoryService, IWebHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            logger.LogInformation("Using memory-only database for development");
            Db = new LiteDatabase(new MemoryStream());
        }
        else
        {
            var dataFile = Path.Combine(directoryService.DataDirectory, "lanfeust.db");
            logger.LogInformation("Using database at {File}", dataFile);
            Db = new LiteDatabase(dataFile);
        }
    }

    public LiteDatabase Db { get; }
}
