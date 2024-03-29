﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Caching;
using Sitecore.Data;

namespace WageWorks.Foundation.Indexing.Cache
{
    public class IndexingCache : CustomCache
    {
        public IndexingCache(long maxSize) : base("WageWorks.Foundation.Indexing", maxSize)
        {
        }

        public object Get(string cacheKey)
        {
            return (object)this.GetObject(cacheKey.ToString());
        }

        public void Set(string cacheKey, object requirementList)
        {
            this.SetObject(cacheKey.ToString(), requirementList);
        }
    }
}