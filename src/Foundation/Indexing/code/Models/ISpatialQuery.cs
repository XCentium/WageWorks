namespace Wageworks.Foundation.Indexing.Models
{
    using Solr.SpatialSearch;
    using System.Collections.Generic;

    public interface ISpatialQuery : IQuery
    {
        SpatialPoint Origin { get; set; }
        double Radius { get; set; }
    }
}