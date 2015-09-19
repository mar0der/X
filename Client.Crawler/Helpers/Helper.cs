namespace Crawler.Helpers
{
    #region

    using System.Collections.Generic;

    using Crawler.Parsers;

    #endregion

    public class Helper
    {
        public static Dictionary<long?, string> GetAllCategories()
        {
            var categories = new Dictionary<long?, string>();
            var categorySource = new Requester().Get("https://itunes.apple.com/us/genre/ios/id36?mt=8");
            var allLinks = Parser.ExtractLinks(categorySource);

            foreach (var link in allLinks)
            {
                if (link.IndexOf("https://itunes.apple.com/us/genre/ios-") == 0)
                {
                    var id = ItunesParser.ExtractId(link);
                    categories.Add(id, link);
                }
            }

            return categories;
        }
    }
}