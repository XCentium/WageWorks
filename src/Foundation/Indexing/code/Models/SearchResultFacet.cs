using System.Collections.Generic;

namespace Wageworks.Foundation.Indexing.Models
{
    public class SearchResultFacet : ISearchResultFacet
    {
        public IEnumerable<ISearchResultFacetValue> Values { get; set; }
        public IQueryFacet Definition { get; set; }
    }
}