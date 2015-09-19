namespace Crawler.Parsers
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    #endregion

    public static class ItunesParser
    {
        public static bool IsApp(string url)
        {
            return url.IndexOf("https://itunes.apple.com/us/app") == 0;
        }

        public static HashSet<string> ExtractItunesLinks(HashSet<string> links)
        {
            var result = new HashSet<string>();

            foreach (var link in links)
            {
                if (IsApp(link))
                {
                    result.Add(link);
                }
            }

            return result;
        }

        public static long? ExtractId(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            var extractHrefPattern = @"\/id(\d+)?\?";
            var regex = new Regex(extractHrefPattern);
            var match = regex.Match(url);
            match.NextMatch();
            var stringId = match.Groups[1].Value;

            long id;
            var isNum = long.TryParse(stringId, out id);

            if (isNum)
            {
                return id;
            }

            return null;
        }

        public static string ExtractName(string content)
        {
            return ExtractItem(content, "name", "h1");
        }

        public static string ExtractDescription(string content)
        {
            return ExtractItem(content, "description", "p");
        }

        public static string ExtractVersion(string content)
        {
            return ExtractItem(content, "softwareVersion", "span");
        }

        public static string ExtractReleaseDate(string content)
        {
            var publishDatePattern = "<span itemprop=\"datePublished\" content=\"(.+?)\">";
            var publishedDate = FirstMatched(content, publishDatePattern);

            return publishedDate;
        }

        public static string ExtractLastUpdateDate(string content)
        {
            var lastUpdatePattern = "<span itemprop=\"datePublished\".+?>(.+?)<\\/span>";
            var lastUpdateDate = FirstMatched(content, lastUpdatePattern);

            return lastUpdateDate;
        }

        public static string ExtractItem(string content, string itemName, string tagName)
        {
            var pattern = "<" + tagName + " itemprop=\"" + itemName + "\">(.+?)<\\/" + tagName + ">";
            var matchedValue = FirstMatched(content, pattern);

            return Parser.RemoveTags(matchedValue);
        }

        public static string FirstMatched(string content, string pattern)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            var regex = new Regex(pattern);
            var match = regex.Match(content);
            match.NextMatch();
            string matchedValue;
            try
            {
                matchedValue = match.Groups[1].Value;
            }
            catch (Exception)
            {
                return null;
            }

            return matchedValue;
        }
    }
}