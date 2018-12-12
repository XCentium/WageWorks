namespace WageWorks.Foundation.Indexing.Models
{
    public class SearchResultFacetValue : ISearchResultFacetValue
    {
        public object Value { get; set; }
        public int Count { get; set; }
        public bool Selected { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set;  }
    }
}