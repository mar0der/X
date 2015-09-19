namespace Tracker.WebApi.Controllers
{
    #region

    using System.Linq;
    using System.Web.Http;

    using Tracker.Data;
    using Tracker.Models.TrackerModels;

    #endregion

    public class BaseController : ApiController
    {
        public BaseController()
        {
            this.Data = new TrackerDbContext();
        }

        public TrackerDbContext Data { get; set; }

        public User CurrentUser
        {
            get
            {
                return this.Data.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
            }
        }
    }
}