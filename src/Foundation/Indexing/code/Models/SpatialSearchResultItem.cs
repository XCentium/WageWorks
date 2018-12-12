using WageWorks.Foundation.Solr.SpatialSearch;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WageWorks.Foundation.Indexing.Models
{
    public class SpatialSearchResultItem : SearchResultItem
    {
        [IndexField("title")]
        public string Title { get; set; }
        [IndexField("body")]
        public string Body { get; set; }
        [IndexField("lat_lon")]
        public SpatialPoint SpatialLocation { get; set; }
    }
}