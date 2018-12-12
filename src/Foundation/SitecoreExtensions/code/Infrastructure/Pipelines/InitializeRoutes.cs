﻿using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace WageWorks.Foundation.SitecoreExtensions.Infrastructure.Pipelines
{
    public class InitializeRoutes
    {
        public void Process(PipelineArgs args)
        {
          
            RouteTable.Routes.IgnoreRoute("{*botdetect}",
                     new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            RouteTable.Routes.MapMvcAttributeRoutes();
        }
    }
}