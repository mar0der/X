namespace Client.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;
    using Tracker.Models.Models;

    public class TrackerDbContext : IdentityDbContext<User>
    {
        public TrackerDbContext()
            : base("TrackerDbContext", throwIfV1Schema: false)
        {
        }
        
        public virtual IDbSet<Job> Jobs { get; set; }


        public static TrackerDbContext Create()
        {
            return new TrackerDbContext();
        }
    }
}
