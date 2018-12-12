using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Xml;
using WageWorks.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Diagnostics;

namespace WageWorks.Foundation.Indexing.Infrastructure.Fields
{
    public class AvailableOnSiteComputedField : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {

            var indexItem = indexable as SitecoreIndexableItem;
            if (indexItem == null) return null;
            bool displayOnSite = false;
            bool isDeleted = false;
            try
            {
                if (indexItem.Item.Template.ID == Templates.ProductVariant.ID)
                {

                    if (MainUtil.GetBool(ProductExtensions.GetFirstTokenValue
                    (JsonExtensions.GetTokenValues(indexItem.Item.Parent, Constants.JsonFields.ProductJsonField,
                        Constants.JsonFields.ProductFields.DisplayOnWebsite)), true)
                        && MainUtil.GetBool(ProductExtensions.GetFirstTokenValue
                        (JsonExtensions.GetTokenValues(indexItem.Item, Constants.JsonFields.SkuJsonField,
                            Constants.JsonFields.SkuFields.DisplayOnWebsite)), true))
                        displayOnSite = true;

                    if (MainUtil.GetBool(indexItem.Item[Templates.ProductVariant.Fields.IsDeleted], false)
                        || MainUtil.GetBool(indexItem.Item.Parent[Templates.Product.Fields.IsDeleted], false))
                        isDeleted = true;
                }

                if (indexItem.Item?.Template.ID == Templates.Product.ID)
                {
                    displayOnSite = MainUtil.GetBool(ProductExtensions.GetFirstTokenValue
                    (JsonExtensions.GetTokenValues(indexItem.Item, Constants.JsonFields.ProductJsonField,
                        Constants.JsonFields.ProductFields.DisplayOnWebsite)), true);

                    isDeleted = MainUtil.GetBool(indexItem.Item[Templates.Product.Fields.IsDeleted], false);
                }

                return (displayOnSite && !isDeleted);
            }
            catch (Exception ex)
            {
                Log.Warn($"Could not retrieve attribute for compute field: Field Name: {this.FieldName}, Item Id: {indexItem.Item.ID}, Item Name: {indexItem.Item.Name},", ex, this);
                return null;
            }
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}