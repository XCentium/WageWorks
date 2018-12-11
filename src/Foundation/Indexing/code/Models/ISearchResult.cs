using System;
using System.Collections.Generic;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;

namespace Wageworks.Foundation.Indexing.Models
{
    public interface ISearchResult
    {
        Item Item { get; }
        string Title { get; set; }
        string ContentType { get; set; }
        string Description { get; set; }
        Uri Url { get; set; }
        string ViewName { get; set; }
        MediaItem Media { get; set; }

        object IndexResult { get; set; }

        string Badge { get; set; }
        string BadgeClass { get; set; }
    }

    public class SearchResultEqualityComparer : IEqualityComparer<ISearchResult>
    {
        public bool Equals(ISearchResult a, ISearchResult b)
        {
            if (a.Item == null || a.Url == null) return false;
            return a.Item.Equals(b.Item) || a.Url.Equals(b.Url);
        }

        public int GetHashCode(ISearchResult obj)
        {
            if (obj.Url == null) return 0;
            return obj.Url.GetHashCode();
        }
    }
}