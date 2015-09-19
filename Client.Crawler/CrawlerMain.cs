using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Diagnostics;
using System.Threading;
using EntityFramework.Extensions;
using Crawler.Helpers;
using System.Web.Script.Serialization;
using Crawler.Dtos;
using Data;
using Models.Models;
using Tracker.Models.Models;
using Client.Data;

namespace Crawler
{
    class CrawlerMain
    {
        public delegate void Shooter(int n);

        static void Main(string[] args)
        {
            var accessToken = GetAccessToken();
            var requester = new Requester();
            requester.Headers.Add("Authorization", "Bearer " + accessToken);
            var jobJson = requester.Get("http://localhost:41167/api/Jobs");
            var js = new JavaScriptSerializer();
            var jobData = js.Deserialize<JobDataDto>(jobJson);
            CrawlJob(jobData);

            //var categories = Helper.GetAllCategories();

            //foreach (var key in categories.Keys)
            //{
            //    CrawlCategory(categories[key]);
            //}
        }

        public static void CrawlJob(JobDataDto job)
        {
            var startPage = job.StartPage + job.CrawledPages;
            var lastPage = job.StartPage + job.PagesCount;
            var dataStorageContext = new DataStorageDbContext();
            TrackerDbContext trackerDbContext = new TrackerDbContext();
            //TODO if last page
            for (int page = startPage; page < lastPage; page++)
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
                    Console.WriteLine("Page: {0}", page);
                    Console.WriteLine("{0} of {1}", passedApps, allAppsLinks.Count);
                    dataStorageContext.Apps.Add(new App()
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
                if(databaseSavingStatus > 0)
                {
                    var currentJob = trackerDbContext.Jobs.FirstOrDefault(j => j.Id == job.Id);
                    currentJob.CrawledPages++;
                    if (currentJob.CrawledPages == job.PagesCount)
                    {
                        trackerDbContext.Jobs.Remove(currentJob);
                    }

                    trackerDbContext.SaveChanges();
                }
            }
        }

        public static void CrawlCategory(string categoryUrl)
        {
            char[] letters = new char[] { 'A', 'B','C','D', 'E','F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 
                'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '*'};

            string currentLink = string.Empty;
            var context = new DataStorageDbContext();
            if (ItunesParser.ExtractId(categoryUrl) == 6018)
            {
                letters = new char[] { '*' };
            }

            foreach (var letter in letters)
            {
                var page = 1;
                while (true)
                {
                    currentLink = string.Format("https://itunes.apple.com/us/genre/ios-books/id6018?mt=8&letter={0}&page={1}#page", letter, page);
                    var content = new Requester().Get(currentLink);
                    var allLinks = Parser.ExtractLinks(content);
                    var allAppsLinks = ItunesParser.ExtractItunesLinks(allLinks);
                    if (allAppsLinks.Count < 2)
                    {
                        break;
                    }

                    foreach (var appLink in allAppsLinks)
                    {
                        var id = ItunesParser.ExtractId(appLink);
                        var json = new Requester().Get("https://itunes.apple.com/lookup?id=" + id);
                        var js = new JavaScriptSerializer();
                        var result = js.Deserialize<AppResultDto>(json);
                        var app = result.Results[0];
                        Console.Clear();
                        Console.WriteLine("{0} - {1} - {2} - {3}", ItunesParser.ExtractId(categoryUrl), letter, page, id);
                        context.Apps.Add(new App()
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
                    }

                    context.SaveChanges();
                    page++;
                }
            }
        }

        public static string GetAccessToken()
        {
            var username = "gosho";
            var passowrd = "123456";
            var grant_type = "password";
            var urlEncodedPost = string.Format("username={0}&password={1}&grant_type={2}", username, passowrd, grant_type);
            var json = new Requester().Post("http://localhost:41167/api/Account/Login", urlEncodedPost);

            var js = new JavaScriptSerializer();
            var userData = js.Deserialize<UserDataDto>(json);

            return userData.access_token;
        }
    }
}
