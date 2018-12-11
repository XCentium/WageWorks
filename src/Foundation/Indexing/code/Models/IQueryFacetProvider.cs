using System.Collections.Generic;

namespace Wageworks.Foundation.Indexing.Models
{
    public interface IQueryFacetProvider
    {
        IEnumerable<IQueryFacet> GetFacets(IQuery query = null);
    }
}