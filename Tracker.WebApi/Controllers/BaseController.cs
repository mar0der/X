using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Tracker.Data;
using Tracker.Models.Models;

namespace Tracker.WebApi.Controllers
{
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