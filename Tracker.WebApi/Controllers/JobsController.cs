namespace Tracker.WebApi.Controllers
{
    #region

    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading;
    using System.Web.Http;

    using EntityFramework.Extensions;

    using Tracker.Data;
    using Tracker.Models.Enum;
    using Tracker.Models.TrackerModels;
    using Tracker.WebApi.Models.Jobs;

    #endregion

    [RoutePrefix("api/jobs")]
    public class JobsController : BaseController
    {
        [Authorize]
        [HttpGet]
        [Route("GetJob")]
        public IHttpActionResult GetJob()
        {
            if (this.CurrentUser == null)
            {
                return this.BadRequest();
            }

            var existingJob =
                this.Data.Jobs.Include(j => j.Category).FirstOrDefault(j => j.UserId == this.CurrentUser.Id);
            if (existingJob != null)
            {
                return this.Ok(JobViewModel.Create(existingJob));
            }

            var highPriorityJobsCount = this.Data.Jobs.Count(j => j.Priority == Priority.High);
            if (highPriorityJobsCount == 0)
            {
                this.Data.Jobs.Update(j => new Job { Priority = Priority.High });

                highPriorityJobsCount = this.Data.Jobs.Count(j => j.Priority == Priority.High);
            }

            var allJobs = this.Data.Jobs.Count();
            if (allJobs < 50)
            {
                new Thread(() => this.GenerateJobs()).Start();
            }

            var rand = new Random();
            var skip = rand.Next(0, highPriorityJobsCount);
            var job =
                this.Data.Jobs.Include(j => j.Category)
                    .Where(j => j.Priority == Priority.High)
                    .OrderBy(j => j.Id)
                    .Skip(skip)
                    .FirstOrDefault();

            if (job == null)
            {
                return this.BadRequest("No avaiiable jobs.");
            }

            job.UserId = this.CurrentUser.Id;
            this.Data.SaveChanges();

            return this.Ok(JobViewModel.Create(job));
        }

        [Authorize]
        [HttpPost]
        [Route("GenerateJobs")]
        public IHttpActionResult GenerateJobs()
        {
            var context = new TrackerDbContext();
            var categoryLetterLastPages = context.CategoryLetterLastPages.ToList();

            if (categoryLetterLastPages.Count == 0)
            {
                return this.BadRequest("No Categories found!. Please run api/categories/CategoryLetterLastPages first!");
            }

            foreach (var categoryLetterLastPage in categoryLetterLastPages)
            {
                var pages = categoryLetterLastPage.LastPage;
                var minPagesPerJob = 2;
                var maxPagesPerJob = 5;
                var startPage = 1;
                var rand = new Random();

                while (startPage <= pages)
                {
                    var jobPages = rand.Next(minPagesPerJob, maxPagesPerJob);
                    var lastPage = jobPages + startPage - 1; // changed
                    if (lastPage > pages)
                    {
                        jobPages = pages - startPage + 1;
                        lastPage = pages;
                    }

                    context.Jobs.Add(
                        new Job
                            {
                                CategoryId = categoryLetterLastPage.CategoryId, 
                                CrawledPages = 0, 
                                Letter = categoryLetterLastPage.Letter, 
                                StartPage = startPage, 
                                PagesCount = jobPages, 
                                IsLast = lastPage == pages, 
                                LastAction = DateTime.Now, 
                                Priority = Priority.Low
                            });

                    startPage = lastPage + 1;
                }

                context.SaveChanges();
            }

            return this.Ok();
        }
    }
}