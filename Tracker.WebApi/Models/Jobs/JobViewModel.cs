namespace Tracker.WebApi.Models.Jobs
{
    #region

    using System;
    using System.Linq.Expressions;

    using Tracker.Models.TrackerModels;

    #endregion

    public class JobViewModel
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string CategoryUrl { get; set; }

        public string Letter { get; set; }

        public int StartPage { get; set; }

        public int PagesCount { get; set; }

        public int CrawledPages { get; set; }

        public bool IsLast { get; set; }

        public static Expression<Func<Job, JobViewModel>> ViewModel
        {
            get
            {
                return
                    model =>
                    new JobViewModel
                        {
                            Id = model.Id, 
                            CategoryId = model.CategoryId, 
                            CategoryUrl = model.Category.Link, 
                            Letter = model.Letter, 
                            StartPage = model.StartPage, 
                            PagesCount = model.PagesCount, 
                            IsLast = model.IsLast
                        };
            }
        }

        public static JobViewModel Create(Job model)
        {
            return new JobViewModel
                       {
                           Id = model.Id, 
                           CategoryId = model.CategoryId, 
                           CategoryUrl = model.Category.Link, 
                           Letter = model.Letter, 
                           StartPage = model.StartPage, 
                           PagesCount = model.PagesCount, 
                           IsLast = model.IsLast
                       };
        }
    }
}