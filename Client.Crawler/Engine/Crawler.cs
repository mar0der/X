namespace Crawler.Engine
{
    #region

    using System;
    using System.Threading;

    using Data;

    using global::Crawler.Helpers;
    using global::Crawler.Parsers;

    #endregion

    internal class Crawler
    {
        private int deep;

        public Crawler(int deep)
        {
            this.Deep = deep;
            this.Level = 1;
            this.Requests = 0;
        }

        public int Requests { get; set; }

        public int Deep
        {
            get
            {
                return this.deep;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Deep cannot be a negative number.");
                }

                this.deep = value;
            }
        }

        public int Level { get; set; }

        public DataStorageDbContext DbContext { get; set; }

        public void Crawl(string pageLink)
        {
            this.Requests++;

            // ItunesDbContext context = new ItunesDbContext();
            var content = new Requester().Get(pageLink);
            var allLinks = Parser.ExtractLinks(content);
            var itunesLinks = ItunesParser.ExtractItunesLinks(allLinks);

            // var links = new HashSet<string>(itunesLinks);
            // Console.WriteLine(pageLink);
            foreach (var link in allLinks)
            {
                // var t = new Thread(() => this.SaveLinkContent(LinkBuilder.CreateLink(pageLink, link)));
                // t.Start();
                if (ItunesParser.IsApp(link))
                {
                    this.SaveLinkContent(link);
                }
            }

            if (this.Level < this.Deep)
            {
                this.Level++;
                foreach (var link in allLinks)
                {
                    var t = new Thread(() => this.Crawl(LinkBuilder.CreateLink(pageLink, link)));
                    t.Start();
                }
            }
        }

        public void SaveLinkContent(string link)
        {
            var id = ItunesParser.ExtractId(link);
            new Requester().Get("https://itunes.apple.com/lookup?id=" + id);
            Console.WriteLine(id);

            // ItunesDbContext context = new ItunesDbContext();
            // DateTime releaseDate, lastUpdate;

            // DateTime.TryParse(ItunesParser.ExtractReleaseDate(pageContent), out releaseDate);
            // DateTime.TryParse(ItunesParser.ExtractLastUpdateDate(pageContent), out lastUpdate);

            // context.Apps.Add(new App()
            // {
            // AppId = ItunesParser.ExtractAppId(link),
            // Name = ItunesParser.ExtractName(pageContent),
            // Version = ItunesParser.ExtractVersion(pageContent),
            // Description = ItunesParser.ExtractDescription(pageContent)
            // });

            // context.SaveChanges();
        }
    }
}