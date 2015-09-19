namespace Tracker.Data
{
    #region

    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    using Tracker.Data.Migrations;
    using Tracker.Models.TrackerModels;

    #endregion

    public class TrackerDbContext : IdentityDbContext<User>
    {
        public TrackerDbContext()
            : base("TrackerDbConnection", false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TrackerDbContext, Configuration>());
        }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<CategoryLetterLastPage> CategoryLetterLastPages { get; set; }

        public virtual IDbSet<Job> Jobs { get; set; }

        public static TrackerDbContext Create()
        {
            return new TrackerDbContext();
        }
    }
}