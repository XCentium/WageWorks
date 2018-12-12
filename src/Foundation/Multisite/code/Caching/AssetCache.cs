using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Caching;
using Sitecore.Data;

namespace WageWorks.Foundation.Multisite.Caching
{
    public class AssetCache : CustomCache
    {
        public AssetCache(long maxSize) : base("WageWorks.Foundation.AssetCache", maxSize)
        {
        }
        public string Get(string cacheKey)
        {
            return (string)this.GetObject(cacheKey.ToString());
        }

        public void Set(string cacheKey, string asset)
        {
            this.SetObject(cacheKey.ToString(), asset);
        }
    }
}