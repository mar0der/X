using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler
{
    public class Parser
    {
        public static HashSet<string> ExtractLinks(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return new HashSet<string>();
            }

            HashSet<string> links = new HashSet<string>();
            if (string.IsNullOrEmpty(content))
            {
                return links;
            }

            string extractHrefPattern = "<a.+?href=\"(.+?)\".*?>.+?</a>";
            Regex regex = new Regex(extractHrefPattern);
            Match match = regex.Match(content);

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
