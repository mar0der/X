namespace Tracker.WebApi.Controllers
{
    #region

    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Http;

    using Crawler;

    using Tracker.Models.TrackerModels;

    #endregion

    [RoutePrefix("api/categories")]
    public class CategoriesController : BaseController
    {
        [HttpPut]
        [Route("UpdateCategories")]

        // TODO: fix below
        // [Authorize(Roles = "admin")] 
        public IHttpActionResult UpdateCategories()
        {
            var pageContent = new Requester().Get("https://itunes.apple.com/us/genre/ios/id36?mt=8");
            var links = Parser.ExtractLinks(pageContent);

            foreach (var link in links)
            {
                if (link.IndexOf("https://itunes.apple.com/us/genre/ios-") == 0)
                {
                    this.Data.Categories.AddOrUpdate(
                        new Category
                            {
                                Id = (int)ItunesParser.ExtractId(link), 
                                Name = ItunesParser.ExtractCategoryName(link), 
                                Link = link
                            });
                }
            }

            this.Data.SaveChanges();

            return this.Ok();
        }

        [HttpPost]
        [Route("UpdateCategoryLetterLastPages")]
        public IHttpActionResult UpdateCategoryLetterLastPages()
        {
            var categories = this.Data.Categories.ToList();

            if (categories.Count == 0)
            {
                return this.BadRequest("There is no categories yet. Please run app/categories/UpdateCategories first!");
            }

            char[] letters =
                {
                    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 
                    'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '*'
                };

            foreach (var category in categories)
            {
                foreach (var letter in letters)
                {
                    var letterString = letter.ToString();
                    var lastIndexedPage =
                        this.Data.CategoryLetterLastPages.FirstOrDefault(
                            c => c.CategoryId == category.Id && c.Letter == letterString);
                    var lastPage = 1;
                    if (lastIndexedPage != null)
                    {
                        lastPage = lastIndexedPage.LastPage;
                    }

                    string currentLink;
                    while (true)
                    {
                        currentLink = string.Format("{0}&letter={1}&page={2}#page", category.Link, letter, lastPage);
                        var content = new Requester().Get(currentLink);
                        var allLinks = Parser.ExtractLinks(content);
                        var allAppsLinks = ItunesParser.ExtractItunesLinks(allLinks);
                        if (allAppsLinks.Count < 2)
                        {
                            break;
                        }

                        lastPage++;
                    }

                    this.Data.CategoryLetterLastPages.AddOrUpdate(
                        new CategoryLetterLastPage
                            {
                                CategoryId = category.Id, 
                                Letter = letterString, 
                                LastPage = lastPage - 1
                            });

                    this.Data.SaveChanges();
                }
            }

            return this.Ok();
        }
    }
}