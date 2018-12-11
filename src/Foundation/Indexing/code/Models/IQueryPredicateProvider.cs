using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;

namespace Wageworks.Foundation.Indexing.Models
{
    public interface IQueryPredicateProvider
    {
        Expression<Func<SearchResultItem, bool>> GetQueryPredicate(IQuery query);
        IEnumerable<ID> SupportedTemplates { get; }
        IEnumerable<ID> SupportedTemplatesWithQuery(IQuery query);
        string ProviderName { get; }
    }
}