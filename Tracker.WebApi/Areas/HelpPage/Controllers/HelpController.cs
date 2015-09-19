namespace Tracker.WebApi.Areas.HelpPage.Controllers
{
    #region

    using System.Web.Http;
    using System.Web.Mvc;

    using Tracker.WebApi.Areas.HelpPage.ModelDescriptions;

    #endregion

    /// <summary>
    ///     The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";

        public HelpController()
            : this(GlobalConfiguration.Configuration)
        {
        }

        public HelpController(HttpConfiguration config)
        {
            this.Configuration = config;
        }

        public HttpConfiguration Configuration { get; private set; }

        public ActionResult Index()
        {
            this.ViewBag.DocumentationProvider = this.Configuration.Services.GetDocumentationProvider();
            return this.View(this.Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

        public ActionResult Api(string apiId)
        {
            if (!string.IsNullOrEmpty(apiId))
            {
                var apiModel = this.Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return this.View(apiModel);
                }
            }

            return this.View(ErrorViewName);
        }

        public ActionResult ResourceModel(string modelName)
        {
            if (!string.IsNullOrEmpty(modelName))
            {
                var modelDescriptionGenerator = this.Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return this.View(modelDescription);
                }
            }

            return this.View(ErrorViewName);
        }
    }
}