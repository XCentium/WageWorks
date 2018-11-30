using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Caching;
using Sitecore.Data;

namespace Wageworks.Foundation.Multisite.Caching
{
    public class UrlCache : CustomCache
    {
        public UrlCache(long maxSize) : base("Wageworks.Foundation.UrlCache", maxSize)
        {
        }
        public string Get(string cacheKey)
        {
            return (string)this.GetObject(cacheKey.ToString());
        }

        public void Set(string cacheKey, string url)
        {
            this.SetObject(cacheKey.ToString(), url);
        }
    }
}