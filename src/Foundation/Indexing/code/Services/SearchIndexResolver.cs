using Sitecore.ContentSearch;
using Wageworks.Foundation.DependencyInjection;

namespace Wageworks.Foundation.Indexing.Services
{
    [Service]
    public class SearchIndexResolver
    {
        public virtual ISearchIndex GetIndex(SitecoreIndexableItem contextItem)
        {
            return ContentSearchManager.GetIndex(contextItem);
        }
    }
}