#region

using Microsoft.Owin;

using Tracker.WebApi;

#endregion

[assembly: OwinStartup(typeof(Startup))]

namespace Tracker.WebApi
{
    #region

    using Owin;

    #endregion

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}