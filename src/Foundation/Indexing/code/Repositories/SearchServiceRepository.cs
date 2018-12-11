using Wageworks.Foundation.DependencyInjection;
using Wageworks.Foundation.Indexing.Models;
using Wageworks.Foundation.Indexing.Services;

namespace Wageworks.Foundation.Indexing.Repositories
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