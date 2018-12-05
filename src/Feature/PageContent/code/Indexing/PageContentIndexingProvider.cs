//using Sitecore.Diagnostics;
//using Wageworks.Foundation.Indexing.Services;

//namespace Wageworks.Feature.PageContent.Indexing
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Configuration.Provider;
//    using System.Linq.Expressions;
//    using Sitecore.ContentSearch.SearchTypes;
//    using Sitecore.Data;
//    using Sitecore.Data.Fields;
//    using Wageworks.Foundation.Dictionary.Repositories;
//    using Wageworks.Foundation.Indexing.Infrastructure;
//    using Wageworks.Foundation.Indexing.Models;
//    using Sitecore.Web.UI.WebControls;

//    public class PageContentIndexingProvider : ProviderBase, ISearchResultFormatter, IQueryPredicateProvider
//    {
//        public Expression<Func<SearchResultItem, bool>> GetQueryPredicate(IQuery query)
//        {
//            var fieldNames = new[] {Templates.HasPageContent.Fields.Title_FieldName,
//                Templates.HasPageContent.Fields.Summary_FieldName,
//                Templates.HasPageContent.Fields.FooterBody_FieldName,
//                Templates.HasPageContent.Fields.Body_FieldName};
//            return GetFreeTextPredicateService.GetFreeTextPredicate(fieldNames, query);
//        }
//        public string ProviderName => "page";
//        public string ContentType => DictionaryPhraseRepository.Current.Get("/Page Content/Search/Content Type", "Page");

//        public IEnumerable<ID> SupportedTemplates => new[]
//        {
//            Templates.HasPageContent.ID
//        };

//        public IEnumerable<ID> SupportedTemplatesWithQuery(IQuery query)
//        {
//            return new ID[0];
//        }

//        public void FormatResult(SearchResultItem item, ISearchResult formattedResult)
//        {
//            try
//            {
//                var contentItem = item.GetItem();
//                formattedResult.Title = FieldRenderer.Render(contentItem, Templates.HasPageContent.Fields.Title.ToString());
//                formattedResult.Description = FieldRenderer.Render(contentItem, Templates.HasPageContent.Fields.Summary.ToString());
//                formattedResult.ViewName = "~/Views/PageContent/PageContentSearchResult.cshtml";
//            }
//            catch (Exception e)
//            {
//                Log.Warn("Could not format search result, item: " + item?.ItemId, e, this);
//            }
          
//        }
//    }
//}