using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WageWorks.Feature.Teasers.Models.Glass;

namespace WageWorks.Feature.Teasers.Models
{
    public class pp :RenderingModel
    {
        public IPromotion datasource { get; set; }

    }
}