using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;

namespace WageWorks.Feature.Teasers.Models
{
    public interface IRendering
    {
        RenderingParameters Parameters { get; }
        IItem Item { get; }

        RenderingItem RenderingItem { get; }
        Rendering Rendering { get; }
    }
}
