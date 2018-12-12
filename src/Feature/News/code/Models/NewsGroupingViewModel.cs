using Sitecore.Data.Items;
using System.Collections.Generic;

namespace WageWorks.Feature.News.Models
{
    public class NewsGroupingViewModel
    {
        public Dictionary<string, IEnumerable<Item>> Groups { get; set; } = new Dictionary<string, IEnumerable<Item>>();
    }
}