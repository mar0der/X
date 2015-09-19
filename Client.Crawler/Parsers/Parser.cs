namespace Crawler.Parsers
{
    #region

    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    #endregion

    public class Parser
    {
        public static HashSet<string> ExtractLinks(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return new HashSet<string>();
            }

            var links = new HashSet<string>();
            if (string.IsNullOrEmpty(content))
            {
                return links;
            }

            var extractHrefPattern = "<a.+?href=\"(.+?)\".*?>.+?</a>";
            var regex = new Regex(extractHrefPattern);
            var match = regex.Match(content);

            while (match.Success)
            {
                links.Add(match.Groups[1].Value);

                match = match.NextMatch();
            }

            return links;
        }

        public static string RemoveTags(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            var newContent = Regex.Replace(content, "<.+?>", string.Empty);

            return content;
        }
    }
}