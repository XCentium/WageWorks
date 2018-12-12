using WageWorks.Foundation.Indexing.Models;
using WageWorks.Foundation.Indexing.Services;

namespace WageWorks.Foundation.Indexing.Repositories
{
    public interface ISearchServiceRepository
    {
        SearchService Get(ISearchSettings searchSettings);
    }
}