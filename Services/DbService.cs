using System.IO;

using LiteDB;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LanfeustBridge.Services
{
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
                logger.LogInformation("Using database at {dataFile}", dataFile);
                Db = new LiteDatabase(dataFile);
            }
        }

        public LiteDatabase Db { get; }
    }
}
