using System.Collections.Generic;
using Sitecore.Data.Items;

namespace Wageworks.Foundation.Indexing.Models
{
    public interface IQuery
    {
        string QueryText { get; set; }
        int NoOfResults { get; set; }
        Dictionary<string, string[]> Facets { get; set; }
        int Page { get; set; }

        string ConfigurationItemId { get; set; }

        bool IncludeProducts { get; set; }

        Item[] InitialFilters { get; set; }
    }
}