namespace Crawler.Dtos
{
    public class JobDataDto
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string CategoryUrl { get; set; }

        public string Letter { get; set; }

        public int StartPage { get; set; }

        public int PagesCount { get; set; }

        public int CrawledPages { get; set; }

        public bool IsLast { get; set; }
    }
}
