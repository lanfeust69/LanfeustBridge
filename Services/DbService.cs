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
                Db = new LiteDatabase(new MemoryStream());
            }
            else
            {
                var dataFile = Path.Combine(directoryService.DataDirectory, "lanfeust.db");
                Db = new LiteDatabase(dataFile);
            }
            Db.Log.Level = Logger.FULL;
            Db.Log.Logging += m => logger.LogInformation(m);
        }

        public LiteDatabase Db { get; }
    }
}
