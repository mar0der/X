using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Tracker.Models.Enum;
using System.Data.Entity;
using Tracker.Models.Models;
using Tracker.WebApi.Models.Jobs;
using EntityFramework.Extensions;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using Tracker.Data;

namespace Tracker.WebApi.Controllers
{
    [RoutePrefix("api/jobs")]
    public class JobsController : BaseController
    {
        [Authorize]
        [HttpGet]
        public IHttpActionResult GetJob()
        {
            if (this.CurrentUser == null)
            {
                return this.BadRequest();
            }

            var existingJob = this.Data.Jobs
                .Include(j => j.Category)
                .FirstOrDefault(j => j.UserId == this.CurrentUser.Id);
            if (existingJob != null)
            {
                return this.Ok(JobViewModel.Create(existingJob));
            }

            var highPriorityJobsCount = this.Data.Jobs.Count(j => j.Priority == Priority.High);
            if (highPriorityJobsCount == 0)
            {
                this.Data.Jobs.Update(j => new Job()
                {
                    Priority = Priority.High
                });

                highPriorityJobsCount = this.Data.Jobs.Count(j => j.Priority == Priority.High);
            }

            var allJobs = this.Data.Jobs.Count();
            if (allJobs < 50)
            {
                new Thread(() => this.GenerateJobs()).Start();
            }

            var rand = new Random();
            var skip = rand.Next(0, highPriorityJobsCount);
            var job = this.Data.Jobs
                .Include(j => j.Category)
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
        public IHttpActionResult GenerateJobs()
        {
            var context = new TrackerDbContext();
            var categoryLetterLastPages = context.CategoryLetterLastPages.ToList();

            foreach (var categoryLetterLastPage in categoryLetterLastPages)
            {
                var pages = categoryLetterLastPage.LastPage;
                var minPagesPerJob = 2;
                var maxPagesPerJob = 5;
                var startPage = 1;
                var rand = new Random();

                while (startPage <= pages)
                {
                    int jobPages = rand.Next(minPagesPerJob, maxPagesPerJob);
                    int lastPage = jobPages + startPage - 1;//changed
                    if (lastPage > pages)
                    {
                        jobPages = pages - startPage + 1;
                        lastPage = pages;
                    }

                    context.Jobs.Add(new Job()
                    {
                        CategoryId = categoryLetterLastPage.CategoryId,
                        CrawledPages = 0,
                        Letter = categoryLetterLastPage.Letter,
                        StartPage = startPage,
                        PagesCount = jobPages,
                        IsLast = lastPage == pages,
                        LastAction = DateTime.Now,
                        Priority = Priority.Low
                        //UserId = this.CurrentUser.Id
                    });

                    startPage = lastPage + 1;
                }

                context.SaveChanges();
            }

            return this.Ok();
        }
    }
}