using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Data.Migrations;
using Tracker.Models.Models;

namespace Tracker.Data
{
    public class TrackerDbContext : IdentityDbContext<User>
    {
        public TrackerDbContext()
            : base("TrackerDbConnection", throwIfV1Schema: false)
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
