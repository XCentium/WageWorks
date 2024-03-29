﻿using System.Collections.Generic;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace WageWorks.Foundation.Indexing.Models
{
    public class IndexedItem : SearchResultItem
    {
        [IndexField(Constants.IndexFields.HasPresentation)]
        public bool HasPresentation { get; set; }

        [IndexField(Templates.IndexedItem.Fields.IncludeInSearchResults_FieldName)]
        public bool ShowInSearchResults { get; set; }

        [IndexField(Constants.IndexFields.AllTemplates)]
        public List<string> AllTemplates { get; set; }

        [IndexField(Constants.IndexFields.HasSearchResultFormatter)]
        public bool HasSearchResultFormatter { get; set; }

        [IndexField(Constants.IndexFields.SearchResultFormatter)]
        public string SearchResultFormatter { get; set; }

        [IndexField(Constants.IndexFields.IsLatestVersion)]
        public bool IsLatestVersion { get; set; }
    }
}