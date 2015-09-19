namespace Data
{
    #region

    using System.Data.Entity;

    using Data.Migrations;

    using Tracker.Models.DataModels;

    #endregion

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