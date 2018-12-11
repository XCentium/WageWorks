using System;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Wageworks.Foundation.SitecoreExtensions.Extensions;

namespace Wageworks.Foundation.Indexing.Models
{
    public class SearchResult : ISearchResult
    {
        private Uri _url;

        public SearchResult(Item item)
        {
            this.Item = item;
        }

        public Item Item { get; }
        public MediaItem Media { get; set; }
        public object IndexResult { get; set; }
        public string Badge { get; set; }
        public string BadgeClass { get; set; }
        public string Title { get; set; }
        public string ContentType { get; set; }
        public string Description { get; set; }

        public Uri Url
        {
            get
            {
                try
                {
                  return this._url ?? new Uri(this.Item.Url(), UriKind.RelativeOrAbsolute);
                }
                catch (Exception e)
                {
                    Log.Error("SearchResult URL ERROR. Could not retrieve URL for: " + this.Item?.Name + " " + this.Item?.ID, e, this);
                    return null;
                }
                
            }
            set
            {
                this._url = value;
            }
        }

        public string ViewName { get; set; }
    }
}