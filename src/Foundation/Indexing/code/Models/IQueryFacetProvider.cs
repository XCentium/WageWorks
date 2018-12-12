using System.Collections.Generic;

namespace WageWorks.Foundation.Indexing.Models
{
    public interface IQueryFacetProvider
    {
        IEnumerable<IQueryFacet> GetFacets(IQuery query = null);
    }
}