using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Wageworks.Foundation.DependencyInjection;
using Wageworks.Foundation.Multisite.Providers;

namespace Wageworks.Foundation.Multisite
{
    [Service]
    public class SiteContext
    {
        private readonly ISiteDefinitionsProvider siteDefinitionsProvider;

        public SiteContext(ISiteDefinitionsProvider siteDefinitionsProvider)
        {
            this.siteDefinitionsProvider = siteDefinitionsProvider;
        }

        public virtual SiteDefinition GetSiteDefinition([NotNull] Item item)
        {
            Assert.ArgumentNotNull(item, nameof(item));

            return this.siteDefinitionsProvider.GetContextSiteDefinition(item);
        }
    }
}