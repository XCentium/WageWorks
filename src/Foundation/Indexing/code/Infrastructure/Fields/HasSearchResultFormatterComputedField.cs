﻿#region using



#endregion

using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using WageWorks.Foundation.Indexing.Repositories;
using WageWorks.Foundation.SitecoreExtensions.Extensions;

namespace WageWorks.Foundation.Indexing.Infrastructure.Fields
{
    public class HasSearchResultFormatterComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexItem = indexable as SitecoreIndexableItem;
            if (indexItem == null)
            {
                return null;
            }
            var item = indexItem.Item;


            if (item.IsDerived(Templates.Product.ID) || item.IsDerived(Templates.ProductVariant.ID))
            {
                return true;
            }

            return IndexingProviderRepository.SearchResultFormatters.Any(p => p.SupportedTemplates.Any(id => item.IsDerived(id)));
        }
    }
}