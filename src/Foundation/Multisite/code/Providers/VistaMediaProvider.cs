
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore;
using Sitecore.Abstractions;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Sites;
using Sitecore.Text;
using Sitecore.Web;
//using Wageworks.Foundation.Commerce.Extensions;
//using Wageworks.Foundation.Commerce.Models;
//using Wageworks.Foundation.Multisite.Caching;
//using Wageworks.Foundation.SitecoreExtensions.Extensions;
using Version = System.Version;

namespace Wageworks.Foundation.Multisite.Providers
{
#pragma warning disable CS0612 // Type or member is obsolete
    public class WageworksMediaProvider : MediaProvider
    {
        //private static readonly AssetCache _cache = new AssetCache(StringUtil.ParseSizeString("200MB"));
        string CDNHostnamePropertyKey = "cdnHostname";

        //public override Media GetMedia(MediaUri mediaUri)
        //{
        //    if (!mediaUri.MediaPath.Contains("/media library/dam/")) return base.GetMedia(mediaUri);

        //    Assert.ArgumentNotNull(mediaUri, "mediaUri");
        //    MediaData mediaData = this.GetMediaData(mediaUri);
        //    return mediaData == null ? base.GetMedia(mediaUri) : this.GetMedia(mediaData);

        //}

        //protected override Media GetMedia(MediaData mediaData)
        //{
        //    Assert.ArgumentNotNull(mediaData, "mediaData");
        //    return MediaManager.Config.ConstructMediaInstance(mediaData);
        //}

        //protected override MediaData GetMediaData(MediaUri mediaUri)
        //{
        //    Assert.ArgumentNotNull(mediaUri, "mediaUri");
        //    Database database = mediaUri.Database;
        //    if (database == null)
        //    {
        //        return null;
        //    }
        //    string mediaPath = mediaUri.MediaPath;
        //    if (string.IsNullOrEmpty(mediaPath))
        //    {
        //        return null;
        //    }

        //    Language language = mediaUri.Language;
        //    if (language == null)
        //    {
        //        language = Context.Language;
        //    }
        //    Sitecore.Data.Version version = mediaUri.Version;
        //    if (version == null)
        //    {
        //        version = Sitecore.Data.Version.Latest;
        //    }

        //    var stringId = String.Empty;
        //    Item item = null;
        //    if (mediaUri.MediaPath.Contains("/media library/dam/"))
        //    {
        //        string cacheKey = $"GetMediaData-{Sitecore.Context.Site.Name}-{mediaPath}";
        //        stringId = _cache.Get(cacheKey);
        //        if (string.IsNullOrWhiteSpace(stringId))
        //        {
        //            var actualId = GetActualMediaItemName(mediaPath);
        //            stringId = actualId.ToString();
        //            _cache.Set(cacheKey, stringId);
        //        }
        //        //mediaPath = LinkExtensions.RemoveLastPartOfPath(mediaPath) + "/" + actualName;
        //        if (!ID.IsID(stringId)) return null;

        //        item = database.GetItem(new ID(stringId), language, version);

        //        if (item == null)
        //        {
        //            return null;
        //        }

        //    }
        //    else
        //    {
        //        item = database.GetItem(mediaPath, language, version);
        //    }

        //    return MediaManager.Config.ConstructMediaDataInstance(item);
        //}

        //private ID GetActualMediaItemName(string path)
        //{
        //    var lastPart = LinkExtensions.GetLastPart(path);
        //    lastPart = Path.GetFileNameWithoutExtension(lastPart);
        //    int entityId;
        //    if (!Int32.TryParse(lastPart, out entityId)) return ID.Null;

        //    using (var context = CreateSearchContext())
        //    {
        //        var asset = context.GetQueryable<AssetResultItem>()
        //            .FirstOrDefault(e => e.EntityId == entityId && e.Language == "en");

        //        var contextItem = asset?.GetItem();

        //        return contextItem?.ID;
        //    }
        //}
        //public IProviderSearchContext CreateSearchContext()
        //{
        //    var productIndex = CommerceExtensions.GetProductIndex();
        //    return ContentSearchManager.GetIndex(productIndex).CreateSearchContext();
        //}

        public override string GetMediaUrl(MediaItem item, MediaUrlOptions options)
        {
            string baseMediaUrl = base.GetMediaUrl(item, options);

            if (HasCDN(Sitecore.Context.Site))
                baseMediaUrl = GetMediaUrl(item, baseMediaUrl);
            //if (HasCDN(Sitecore.Context.Site) || baseMediaUrl.Contains(Sitecore.Configuration.Settings.Media.MediaLinkPrefix + "dam/"))
            //             baseMediaUrl = GetMediaUrl(item, baseMediaUrl);

            //if (HasCDN(Sitecore.Context.Site))
            //{
            //var cacheKey = $"GetMediaUrl-{Sitecore.Context.Site.Name}-{baseMediaUrl}";
            //baseMediaUrl = GetMediaUrl(item, baseMediaUrl);

            //var cacheUrl = _cache.Get(cacheKey);
            //if (string.IsNullOrWhiteSpace(cacheUrl))
            //{
            //    cacheUrl = GetMediaUrl(item, baseMediaUrl);
            //    _cache.Set(cacheKey, cacheUrl);
            //}

            //baseMediaUrl = cacheUrl;






            //string cacheKey = string.Concat(Sitecore.Context.Site.Name, baseMediaUrl);

            //var cache = ServiceLocator.ServiceProvider.GetService<ICacheLogic>();
            //if (cache != null)
            //{
            //    var url = baseMediaUrl;
            //    baseMediaUrl = cache.GetOrCreateItem(cacheKey, () => GetMediaUrl(item, url),
            //        new TimeSpan(6, 0, 0));
            //}
            //}
            return LinkExtensions.RemoveSslPort(baseMediaUrl);
        }
        private string GetMediaUrl(MediaItem item, string baseMediaUrl)
        {


            if (HasCDN(Sitecore.Context.Site))
            {
                if (baseMediaUrl.Contains(Sitecore.Configuration.Settings.Media.MediaLinkPrefix))
                {
                    baseMediaUrl = LinkExtensions.RemoveSslPort(baseMediaUrl);
                    var serverAddresss = LinkExtensions.RemoveSslPort(WebUtil.GetServerUrl());


                    var cdnUrl = Sitecore.Context.Site.Properties[CDNHostnamePropertyKey];
                    baseMediaUrl = baseMediaUrl.Replace(serverAddresss,
                        cdnUrl);

                    if (!baseMediaUrl.StartsWith(cdnUrl))
                        baseMediaUrl = cdnUrl + baseMediaUrl;

                    Log.Info($"GetMediaUrl: baseMediaUrl: {baseMediaUrl}, Server address: {serverAddresss}", this);
                    if (!string.IsNullOrWhiteSpace(baseMediaUrl) && Uri.IsWellFormedUriString(baseMediaUrl, UriKind.RelativeOrAbsolute))
                    {
                        var uri = new Uri(baseMediaUrl, UriKind.RelativeOrAbsolute);
                        //UrlString url = new UrlString(baseMediaUrl);

                        var version = item.InnerItem.Version.Number.ToString();
                        var updated = DateUtil.ToIsoDate(item.InnerItem.Statistics.Updated);
                        uri = uri.AddParameter("v", version);
                        uri = uri.AddParameter("d", updated);

                        //url.HostName = Sitecore.Context.Site.Properties[CDNHostnamePropertyKey];
                        //url.Protocol = WebUtil.GetScheme();
                        //url.Add("v", version);
                        //url.Add("d", updated);
                        baseMediaUrl = uri.ToString();
                    }


                }
            }

            //if (baseMediaUrl.Contains(Sitecore.Configuration.Settings.Media.MediaLinkPrefix + "dam/"))
            //{
            //    //replace last segment with entity Id
            //    var isRelative = Uri.IsWellFormedUriString(baseMediaUrl, UriKind.Relative);
            //    if (isRelative)
            //        baseMediaUrl = WebUtil.GetServerUrl() + baseMediaUrl;

            //    var isLinkOk = Uri.IsWellFormedUriString(baseMediaUrl, UriKind.Absolute);

            //    if (!isLinkOk) return LinkExtensions.RemoveSslPort(baseMediaUrl);

            //    var link = new Uri(baseMediaUrl);
            //    var query = link.Query;
            //    var noLastSegment = LinkExtensions.RemoveLastPart(link.AbsolutePath, link);
            //    string entityId = item.InnerItem[Templates.DAMItem.Fields.EntityId];
            //    if (!string.IsNullOrWhiteSpace(entityId))
            //        baseMediaUrl = $"{noLastSegment}/{entityId}.{item.Extension}{query}";
            //}

            return LinkExtensions.RemoveSslPort(baseMediaUrl);
        }

        private bool HasCDN(Sitecore.Sites.SiteContext siteContext)
        {
            try
            {
                return !string.IsNullOrEmpty(siteContext?.Properties[CDNHostnamePropertyKey]);
            }
            catch (Exception ex)
            {
                Log.Warn("Could not read CDN Hostname property of site definition in MediaProvider", ex, this);
                return false;
            }

        }


    }
#pragma warning restore CS0612 // Type or member is obsolete
}