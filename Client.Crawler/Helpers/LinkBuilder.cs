namespace Crawler.Helpers
{
    #region

    using System.Text.RegularExpressions;

    #endregion

    public static class LinkBuilder
    {
        public static string GetClearLink(string url)
        {
            var pattern = @":\/\/\w+?\.\w+?\/?.+?(\/\w+\.\w+)";
            var regex = new Regex(pattern);
            var match = regex.Match(url);
            match.NextMatch();
            var file = match.Groups[1].Value;
            var result = url;
            if (file.Length > 0)
            {
                result = result.Replace(file, string.Empty);
            }

            return result;
        }

        public static string CreateLink(string link, string path)
        {
            if (path.IndexOf("://") != -1)
            {
                return path;
            }

            var clearLink = GetClearLink(link);

            if (clearLink.EndsWith("/"))
            {
                clearLink = clearLink.Substring(0, clearLink.Length - 1);
            }

            if (path.StartsWith("/"))
            {
                path = path.Substring(1, path.Length - 1);
            }

            return string.Format("{0}/{1}", clearLink, path);
        }
    }
}