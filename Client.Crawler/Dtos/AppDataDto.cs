using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler.Dtos
{
    public class AppDataDto
    {
        public string ArtworkUrl60 { get; set; }

        public string ArtistViewUrl { get; set; }

        public List<string> ScreenshotUrls { get; set; }

        public List<string> IpadScreenshotUrls { get; set; }

        public string ArtworkUrl512 { get; set; }

        public string Kind { get; set; }

        public List<string> Features { get; set; }

        public List<string> SupportedDevices { get; set; }

        public List<string> Advisories { get; set; }

        public bool IsGameCenterEnabled { get; set; }

        public string TrackCensoredName { get; set; }

        public string TrackViewUrl { get; set; }

        public string ContentAdvisoryRating { get; set; }

        public string ArtworkUrl100 { get; set; }

        public List<string> LanguageCodesISO2A { get; set; }

        public string FileSizeBytes { get; set; }

        public string SellerUrl { get; set; }

        public string TrackContentRating { get; set; }

        public string Currency { get; set; }

        public string WrapperType { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }

        public string ArtistId { get; set; }

        public string ArtistName { get; set; }

        public List<string> Genres { get; set; }

        public decimal Price { get; set; }

        public List<int> GenreIds { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string SellerName { get; set; }

        public string BundleId { get; set; }

        public string ReleaseNotes { get; set; }

        public string TrackId { get; set; }

        public string TrackName { get; set; }

        public string PrimaryGenreName { get; set; }

        public int PrimaryGenreId { get; set; }

        public bool IsVppDeviceBasedLicensingEnabled { get; set; }

        public string MinimumOsVersion { get; set; }

        public string FormattedPrice { get; set; }

        public double AverageUserRating { get; set; }

        public int UserRatingCount { get; set; }
    }
}
