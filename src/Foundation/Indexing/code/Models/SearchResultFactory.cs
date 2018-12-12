using System.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using WageWorks.Foundation.Indexing.Repositories;
using WageWorks.Foundation.SitecoreExtensions.Extensions;

namespace WageWorks.Foundation.Indexing.Models
{
    public class SearchResultFactory
    {
        public static ISearchResult Create(SearchResultItem result)
        {
            var item = result.GetItem();
            var formattedResult = new SearchResult(item);
            FormatResultUsingFirstSupportedProvider(result, item, formattedResult);
            return formattedResult;
        }

        private static void FormatResultUsingFirstSupportedProvider(SearchResultItem result, Item item, ISearchResult formattedResult)
        {
            var formatter = FindFirstSupportedFormatter(item) ?? IndexingProviderRepository.DefaultSearchResultFormatter;
            formattedResult.ContentType = formatter.ContentType;
            formatter.FormatResult(result, formattedResult);
        }

        private static ISearchResultFormatter FindFirstSupportedFormatter(Item item)
        {
            if (item.IsDerived(Templates.Product.ID) || item.IsDerived(Templates.ProductVariant.ID))
            {
                var searchResultFormatter = IndexingProviderRepository.SearchResultFormatters.FirstOrDefault(provider =>
                    provider.ContentType == "Products");
                if (searchResultFormatter != null) return searchResultFormatter;
            }
            return IndexingProviderRepository.SearchResultFormatters.FirstOrDefault(provider => provider.SupportedTemplates.Any(item.IsDerived));
            
        }
    }
}