using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WageWorks.Feature.Teasers.Models
{
    public interface IRenderingContext
    {
        IRendering Rendering { get; }
    }
}