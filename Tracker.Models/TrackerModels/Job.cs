namespace Tracker.Models.TrackerModels
{
    #region

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Tracker.Models.Enum;

    #endregion

    public class Job
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string Letter { get; set; }

        public int StartPage { get; set; }

        public int PagesCount { get; set; }

        public int CrawledPages { get; set; }

        public bool IsLast { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public DateTime LastAction { get; set; }

        public Priority Priority { get; set; }
    }
}