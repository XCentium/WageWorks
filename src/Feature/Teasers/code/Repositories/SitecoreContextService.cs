using Glass.Mapper.Sc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WageWorks.Feature.Teasers.Models;

namespace WageWorks.Feature.Teasers.Repositories
{
    public class SitecoreContextService :ISitecoreContextService
    {
        internal readonly ISitecoreContext SitecoreContext;
        public SitecoreContextService(ISitecoreContext sitecoreContext)
        {
            SitecoreContext = sitecoreContext;
        }

        /// <summary>
        /// Determines whether [is rendering fully defined] [the specified rendering].
        /// </summary>
        /// <param name="rendering">The rendering.</param>
        /// <returns></returns>
        internal static bool IsRenderingNotFullyDefined(IRendering rendering)
        {
            return rendering == null || rendering.Rendering == null || rendering.Rendering.Item == null;
        }
        /// <summary>
        /// Ifs the item defined.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        internal static bool IfItemNotDefined(IItem item)
        {
            return item == null || item.Item == null;
        }
    }

}
