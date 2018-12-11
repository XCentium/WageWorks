using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Wageworks.Foundation.SitecoreExtensions.Extensions;

namespace Wageworks.Foundation.Indexing.Services
{
    public static class IndexExtensions
    {
        public static bool IsAvailableOnSite(Item item)
        {

            if (item == null) return false;
            try
            {
                var displayOnSite = false;
                var isDeleted = false;
                if (item.Template.ID == Templates.ProductVariant.ID)
                {

                    if (MainUtil.GetBool(ProductExtensions.GetFirstTokenValue
                        (JsonExtensions.GetTokenValues(item.Parent, Constants.JsonFields.ProductJsonField,
                            Constants.JsonFields.ProductFields.DisplayOnWebsite)), true)
                        && MainUtil.GetBool(ProductExtensions.GetFirstTokenValue
                        (JsonExtensions.GetTokenValues(item, Constants.JsonFields.SkuJsonField,
                            Constants.JsonFields.SkuFields.DisplayOnWebsite)), true))
                        displayOnSite = true;

                    if (MainUtil.GetBool(item[Templates.ProductVariant.Fields.IsDeleted], false)
                        || MainUtil.GetBool(item.Parent[Templates.Product.Fields.IsDeleted], false))
                        isDeleted = true;
                }
                else if (item?.Template.ID == Templates.Product.ID)
                {
                    displayOnSite = MainUtil.GetBool(ProductExtensions.GetFirstTokenValue
                    (JsonExtensions.GetTokenValues(item, Constants.JsonFields.ProductJsonField,
                        Constants.JsonFields.ProductFields.DisplayOnWebsite)), true);

                    isDeleted = MainUtil.GetBool(item[Templates.Product.Fields.IsDeleted], false);
                }
                else
                {
                    displayOnSite = true;
                }

                return (displayOnSite && !isDeleted);
            }
            catch (Exception ex)
            {
                Log.Error($"Could not determine site availability: Item Id: {item.ID}, Item Name: {item.Name},", ex, typeof(IndexExtensions));
                return false;
            }
        }
    }
}