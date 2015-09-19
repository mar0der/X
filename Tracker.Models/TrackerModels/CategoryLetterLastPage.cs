using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracker.Models.Models
{
    public class CategoryLetterLastPage
    {
        [Key, Column(Order = 0)]
        public int CategoryId { get; set; }

        [Key, Column(Order = 1)]
        public string Letter { get; set; }

        public int LastPage { get; set; }

    }
}
