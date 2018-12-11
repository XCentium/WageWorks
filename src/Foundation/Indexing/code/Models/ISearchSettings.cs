using System.Collections.Generic;
using Sitecore.Data;

namespace Wageworks.Foundation.Indexing.Models
{
    public interface ISearchSettings : IQueryRoot
    {
        IEnumerable<ID> Templates { get; }
        bool MustHaveFormatter { get; }
    }
}