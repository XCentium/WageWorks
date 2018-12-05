using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using Wageworks.Foundation.SitecoreExtensions.Extensions;
using Wageworks.Foundation.SitecoreExtensions.Pipelines.HttpRequest;

namespace Wageworks.Foundation.Multisite.Pipelines.HttpRequest
{
    public class WageworksItemResolver : SiteSpecificBaseHttpRequestProcessor
    {
        public string DatabaseName
        {
            get
            {
                var db = Sitecore.Context.Database ?? Sitecore.Context.ContentDatabase;
                return db.Name.ToLowerInvariant();
            }
        }

        public string SiteName
        {
            get
            {
                var site = Sitecore.Context.Site;
                return site?.Name.ToLowerInvariant() ?? "website";
            }
        }

        protected override void SiteSpecificProcess(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            // In Case Sitecore has already mapped the item, do not do anything and simply return
            if (Sitecore.Context.Item != null
                || Sitecore.Context.Database == null
                || args.Url.ItemPath.Length == 0) return;

            if (args.Url.ItemPath.Contains(Sitecore.Configuration.Settings.Media.MediaLinkPrefix))
            {
                var lastPart = LinkExtensions.GetLastPart(args.RequestUrl.AbsolutePath);
                int entityId;
                if (!Int32.TryParse(lastPart, out entityId)) return;

                using (var context = CreateSearchContext())
                {
                    var asset = context.GetQueryable<AssetResultItem>()
                        .FirstOrDefault(e => e.EntityId == entityId && e.Language == "en");

                    var contextItem = asset?.GetItem();
                    if (contextItem == null) return;

                    Sitecore.Context.Item = contextItem;
                    Sitecore.Context.Items.Add("currentEntity", asset);
                }

            }
        }

        public IProviderSearchContext CreateSearchContext()
        {
            var productIndex = CommerceExtensions.GetProductIndex();
            return ContentSearchManager.GetIndex(productIndex).CreateSearchContext();
        }
    }
}