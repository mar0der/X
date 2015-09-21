namespace Crawler
{
    #region

    using System;
    using System.Linq;
    using System.Web.Script.Serialization;

    using Client.Data;

    using Crawler.Dtos;
    using Crawler.Helpers;
    using Crawler.Parsers;
    using Crawler.Configuration;
    using System.Data.Entity.Migrations;
    using EntityFramework.Extensions;

    using Data;

    using Tracker.Models.DataModels;

    #endregion

    internal class CrawlerMain
    {
        public delegate void Shooter(int n);

        public static void Main(string[] args)
        {
            // TODO: put this in a real engine
            var requester = new Requester();
            var js = new JavaScriptSerializer();
            string accessToken = null;
            string jobJson;
            UserManager userManger = null;
            while (true)
            {
                if (accessToken == null)
                {
                    userManger = new UserManager();
                    Console.Write("Username: ");
                    var username = Console.ReadLine();
                    Console.Write("Password: ");
                    var password = Console.ReadLine();
                    if (userManger.Login(username, password))
                    {
                        accessToken = userManger.AccessToken;
                        requester.Headers.Add("Authorization", "Bearer " + accessToken);
                    }
                    else
                    {
                        Console.WriteLine("Unable to login.");
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(accessToken))
                {
                    jobJson = requester.Get(CrawlerConfig.TrackerDomain + "api/Jobs/GetJob");
                    if (jobJson != null)
                    {
                        var jobData = js.Deserialize<JobDataDto>(jobJson);
                        CrawlJob(jobData, userManger.Username);

                    }
                    else
                    {
                        Console.WriteLine("There is no jobs at the moment please try again later");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Could not login. Please check your credentials or internet connection!");
                    break;
                }
            }
        }

        public static void CrawlJob(JobDataDto job, string username)
        {
            var startPage = job.StartPage + job.CrawledPages;
            var lastPage = job.StartPage + job.PagesCount - 1;
            var dataStorageContext = new DataStorageDbContext();
            var trackerDbContext = new TrackerDbContext();

            //TODO if last page and fix the other warnigns
            for (var page = startPage; page <= lastPage; page++)
            {
                var link = string.Format("{0}&letter={1}&page={2}", job.CategoryUrl, job.Letter, page);
                var content = new Requester().Get(link);
                var allLinks = Parser.ExtractLinks(content);
                var allAppsLinks = ItunesParser.ExtractItunesLinks(allLinks);
                var passedApps = 0;
                foreach (var appUrl in allAppsLinks)
                {
                    var appId = ItunesParser.ExtractId(appUrl);
                    var json = new Requester().Get("https://itunes.apple.com/lookup?id=" + appId);
                    var js = new JavaScriptSerializer();
                    var result = js.Deserialize<AppResultDto>(json);
                    var app = result.Results[0];
                    Console.Clear();
                    Console.WriteLine("Hello, {0}", username);
                    Console.WriteLine(new string('-', 20));
                    Console.WriteLine("Category: {0}", job.CategoryId);
                    Console.WriteLine("Letter: {0}", job.Letter);
                    Console.WriteLine("Page: {0} of {1}", page, lastPage);
                    Console.WriteLine("Application: {0} of {1}", passedApps, allAppsLinks.Count);
                    Console.WriteLine("App name: {0}", app.TrackName);
                    dataStorageContext.Apps.AddOrUpdate(
                        new App
                            {
                                TrackId = app.TrackId, 
                                TrackName = app.TrackName, 
                                ArtistId = app.ArtistId, 
                                ArtistName = app.ArtistName, 
                                ArtistViewUrl = app.ArtistViewUrl, 
                                ArtworkUrl100 = app.ArtworkUrl100, 
                                ArtworkUrl512 = app.ArtworkUrl512, 
                                ArtworkUrl60 = app.ArtworkUrl60, 
                                AverageUserRating = app.AverageUserRating, 
                                BundleId = app.BundleId, 
                                ContentAdvisoryRating = app.ContentAdvisoryRating, 
                                Currency = app.Currency, 
                                Description = app.Description, 
                                FileSizeBytes = app.FileSizeBytes, 
                                FormattedPrice = app.FormattedPrice, 
                                IsGameCenterEnabled = app.IsGameCenterEnabled, 
                                IsVppDeviceBasedLicensingEnabled = app.IsVppDeviceBasedLicensingEnabled, 
                                Kind = app.Kind, 
                                MinimumOsVersion = app.MinimumOsVersion, 
                                Price = app.Price, 
                                PrimaryGenreId = app.PrimaryGenreId, 
                                PrimaryGenreName = app.PrimaryGenreName, 
                                ReleaseDate = app.ReleaseDate, 
                                ReleaseNotes = app.ReleaseNotes, 
                                SellerName = app.SellerName, 
                                SellerUrl = app.SellerUrl, 
                                TrackCensoredName = app.TrackCensoredName, 
                                TrackContentRating = app.TrackContentRating, 
                                TrackViewUrl = app.TrackViewUrl, 
                                UserRatingCount = app.UserRatingCount, 
                                Version = app.Version, 
                                WrapperType = app.WrapperType
                            });
                    passedApps++;
                }

                passedApps = 0;
               
                var databaseSavingStatus = dataStorageContext.SaveChanges();
                var currentJob = trackerDbContext.Jobs.FirstOrDefault(j => j.Id == job.Id);
                if (content.Contains("Popular Apps") && allAppsLinks.Count == 0)
                {
                    // TODO: Fix last page in CategoryLetterLastPages
                    trackerDbContext.Jobs.Remove(currentJob);
                    trackerDbContext.SaveChanges();
                    continue;
                }
                if (currentJob == null)
                {
                    continue;
                }

                if (databaseSavingStatus > 0)
                {
                    currentJob.CrawledPages++;
                    currentJob.LastAction = DateTime.Now;
                    if (currentJob.CrawledPages == job.PagesCount)
                    {
                        trackerDbContext.Jobs.Remove(currentJob);
                    }

                    trackerDbContext.SaveChanges();
                }
            }
        }

    }
}