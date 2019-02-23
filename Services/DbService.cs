using System.IO;

using LiteDB;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace LanfeustBridge.Services
{
    public class DbService
    {
        public DbService(ILogger<DbService> logger, DirectoryService directoryService, IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                logger.LogInformation("Using memory-only database for development");
                Db = new LiteDatabase(new MemoryStream());
            }
            else
            {
                var dataFile = Path.Combine(directoryService.DataDirectory, "lanfeust.db");
                logger.LogInformation("Using database at {dataFile}", dataFile);
                Db = new LiteDatabase(dataFile);
            }
            Db.Log.Level = Logger.FULL;
            // strip the timestamp, which is duplicated by the actual loggers
            Db.Log.Logging += m => logger.LogInformation("[LiteDB] " + m.Substring("HH:mm:ss.ffff ".Length));
        }

        public LiteDatabase Db { get; }
    }
}
