namespace Data
{
    using Data.Migrations;
    using Models.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DataStorageDbContext : DbContext
    {
        public DataStorageDbContext()
            : base("DataStorageDbContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataStorageDbContext, Configuration>());
        }

        public IDbSet<App> Apps { get; set; }
    }
}
