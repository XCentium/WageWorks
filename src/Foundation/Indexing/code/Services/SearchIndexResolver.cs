using Sitecore.ContentSearch;
using WageWorks.Foundation.DependencyInjection;

namespace WageWorks.Foundation.Indexing.Services
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