using WageWorks.Foundation.DependencyInjection;
using WageWorks.Foundation.Indexing.Models;
using WageWorks.Foundation.Indexing.Services;

namespace WageWorks.Foundation.Indexing.Repositories
{
    [Service(typeof(ISearchServiceRepository))]
    public class SearchServiceRepository : ISearchServiceRepository
    {
        public virtual SearchService Get(ISearchSettings settings)
        {
            return new SearchService(settings);
        }
    }
}