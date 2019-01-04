using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WageWorks.Feature.Teasers.Models
{
    public class RenderingContextWrapper : IRenderingContext
    {
        public IRendering Rendering
        {
            get
            {
                var renderingContext = RenderingContext.CurrentOrNull;
                return renderingContext != null ? new RenderingWrapper(RenderingContext.CurrentOrNull.Rendering) : null;
            }
        }
    }
}