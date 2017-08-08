using LiteDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LanfeustBridge.Services
{
    public class DbService
    {
        public DbService(DirectoryService directoryService, IHostingEnvironment hostingEnvironment)
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
        }

        public LiteDatabase Db { get; }
    }
}
