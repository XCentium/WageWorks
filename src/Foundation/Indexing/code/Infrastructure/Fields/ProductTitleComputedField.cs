using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using WageWorks.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Diagnostics;

namespace WageWorks.Foundation.Indexing.Infrastructure.Fields
{
    public class ProductTitleComputedField : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            var item = indexable as SitecoreIndexableItem;
            if (item?.Item == null) return null;

            try
            {
                string title = string.Empty;

                if (item.Item.Template.ID == Templates.Product.ID)
                {
                    title = JsonExtensions.GetTokenValues(item, Constants.JsonFields.ProductJsonField,
                        Constants.JsonFields.ProductFields.ClassificationName)?.FirstOrDefault();

                }
                else if (item.Item.Template.ID == Templates.ProductVariant.ID)
                {
                    title = JsonExtensions.GetTokenValues(item, Constants.JsonFields.ProductJsonField,
                        Constants.JsonFields.ProductFields.ClassificationName)?.FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(title))
                        title = JsonExtensions.GetTokenValues(item, Constants.JsonFields.SkuJsonField,
                            Constants.JsonFields.SkuFields.SkuName)?.FirstOrDefault();
                }

                return title;
            }
            catch (Exception ex)
            {
                Log.Warn($"Could not retrieve attribute for compute field: Field Name: {this.FieldName}, Item Id: {item.Item.ID}, Item Name: {item.Item.Name}", ex, this);
                return null;
            }
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}