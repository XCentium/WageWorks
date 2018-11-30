using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Web;
using Wageworks.Foundation.Multisite.Caching;
using Wageworks.Foundation.SitecoreExtensions.Extensions;

namespace Wageworks.Foundation.Multisite.Providers
{
    public class WageworksLinkProvider : LinkProvider
    {
        private static readonly UrlCache _cache = new UrlCache(StringUtil.ParseSizeString("200MB"));

        public string GetItemUrl(string rootItemUrl, Item item, UrlOptions options)
        {
            var url = string.Empty;
            var cacheKey = $"UrlCache-{rootItemUrl}-{item.ID.Guid}";

            if (item?.Template.ID == Templates.ProductVariant.ID || item?.Template.ID == Templates.Product.ID)
                url = _cache.Get(cacheKey);

            if (string.IsNullOrWhiteSpace(url))
            {
                if (!string.IsNullOrEmpty(rootItemUrl) && item != null)
                {
                    if (item?.Template.ID == Templates.ProductVariant.ID)
                    {
                        var category = ItemUtil.ProposeValidItemName(ProductExtensions.GetTopLevelCategory(
                            JsonExtensions.GetTokenValues(item, Constants.JsonFields.ProductJsonField,
                                Constants.JsonFields.ProductFields.PathwayToProductGroup))).ToLowerInvariant();

                        var productFamily = ItemUtil.ProposeValidItemName(ProductExtensions.GetProductFamily(
                            JsonExtensions.GetTokenValues(item, Constants.JsonFields.ProductJsonField,
                                Constants.JsonFields.ProductFields.PathwayToProductGroup))).ToLowerInvariant();

                        var productGroup = ItemUtil.ProposeValidItemName(ProductExtensions.GetFirstTokenValue(
                            JsonExtensions.GetTokenValues(item, Constants.JsonFields.ProductJsonField,
                                Constants.JsonFields.ProductFields.ProductGroupName))).ToLowerInvariant();

                        var skuNumber = ItemUtil.ProposeValidItemName(ProductExtensions.GetFirstTokenValue(
                            JsonExtensions.GetTokenValues(item, Constants.JsonFields.SkuJsonField,
                                Constants.JsonFields.SkuFields.SkuNumber))).ToLowerInvariant();

                        // /en/products/rifle/Fusion/Fusion-Lite/<Item.Sku>
                        url = $"{rootItemUrl}/{category}/{productFamily}/{productGroup}/{skuNumber}".ToLowerInvariant()
                            .Replace(" ", "-");

                        _cache.Set(cacheKey, url);
                    }
                    else if (item?.Template.ID == Templates.Product.ID)
                    {
                        // /en/firearms/a-series/a17
                        var categoryName = ItemUtil.ProposeValidItemName(item.Parent?.Parent?.DisplayName);
                        var parentname = ItemUtil.ProposeValidItemName(item.Parent.DisplayName);
                        var itemname = ItemUtil.ProposeValidItemName(item.DisplayName);
                        url = $"{rootItemUrl}/{categoryName}/{parentname}/{itemname}".ToLowerInvariant().Replace(" ", "-");

                        _cache.Set(cacheKey, url);
                    }
                }
            }

            return string.IsNullOrWhiteSpace(url) ? base.GetItemUrl(item, options) : url.ToLowerInvariant();
        }

        public override string GetItemUrl(Item item, UrlOptions options)
        {
            try
            {
                Sitecore.Sites.SiteContext siteInfo = Sitecore.Sites.SiteContext.Current;
                Item rootItem = null;
                if (!string.IsNullOrEmpty(siteInfo?.Properties["productsRoot"]))
                {
                    rootItem = Sitecore.Context.Database?.GetItem(new ID(siteInfo.Properties["productsRoot"]));
                }

                if (rootItem != null)
                {
                    var rootItemUrl = base.GetItemUrl(rootItem, options);
                    return GetItemUrl(rootItemUrl, item, options);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Could not get URL for Item: {item?.ID}", e, this);
            }

            return base.GetItemUrl(item, options);
        }

    }
}