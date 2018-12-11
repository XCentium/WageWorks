using Wageworks.Foundation.Indexing.Models;
using Wageworks.Foundation.Indexing.Services;

namespace Wageworks.Foundation.Indexing.Repositories
{
    public interface ISearchServiceRepository
    {
        SearchService Get(ISearchSettings searchSettings);
    }
}