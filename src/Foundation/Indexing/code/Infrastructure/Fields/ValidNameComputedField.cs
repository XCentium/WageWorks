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
    public class ValidNameComputedField : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexItem = indexable as SitecoreIndexableItem;
            if (indexItem == null || indexItem.Item == null) return null;
            try
            {
                if (indexItem.Item.Template.ID == Templates.ProductVariant.ID)
                {
                    var skuNumber = ItemUtil.ProposeValidItemName(ProductExtensions.GetFirstTokenValue
                    (JsonExtensions.GetTokenValues(indexItem.Item, Constants.JsonFields.SkuJsonField,
                        Constants.JsonFields.SkuFields.SkuNumber))).ToLowerInvariant();

                    return skuNumber.Replace(" ", "-");
                }

                if (indexItem.Item?.Template.ID == Templates.Product.ID)
                {
                    return ItemUtil.ProposeValidItemName(indexItem.Item.DisplayName).Replace(" ", "-").ToLowerInvariant();
                }

                return null;
            }
            catch (Exception ex)
            {
                Log.Warn($"Could not retrieve attribute for compute field: Field Name: {this.FieldName}, Item Id: {indexItem.Item.ID}, Item Name: {indexItem.Item.Name}", ex, this);
                return null;
            }
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}