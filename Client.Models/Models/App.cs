namespace Models.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class App
    {
        [Key]
        public int Id { get; set; }

        public string ArtworkUrl60 { get; set; }

        public string ArtistViewUrl { get; set; }

        public string ArtworkUrl512 { get; set; }

        public string Kind { get; set; }

        public bool IsGameCenterEnabled { get; set; }

        public string TrackCensoredName { get; set; }

        public string TrackViewUrl { get; set; }

        public string ContentAdvisoryRating { get; set; }

        public string ArtworkUrl100 { get; set; }

        public string FileSizeBytes { get; set; }

        public string SellerUrl { get; set; }

        public string TrackContentRating { get; set; }

        public string Currency { get; set; }

        public string WrapperType { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }

        public string ArtistId { get; set; }

        public string ArtistName { get; set; }

        public decimal Price { get; set; }

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
