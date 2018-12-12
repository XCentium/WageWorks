﻿using System;
using Sitecore.Events.Hooks;

namespace WageWorks.Foundation.SitecoreExtensions.Events.ClearCache
{
    public class ClearCacheHook : IHook
    {
        public void Initialize()
        {
            Sitecore.Eventing.EventManager.Subscribe<ClearCacheEvent>(
                new Action<ClearCacheEvent>(ClearCacheEventHandler.Run));
        }
    }
}
