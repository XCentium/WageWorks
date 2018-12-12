using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using WageWorks.Foundation.DependencyInjection;
using WageWorks.Foundation.Multisite.Providers;

namespace WageWorks.Foundation.Multisite
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