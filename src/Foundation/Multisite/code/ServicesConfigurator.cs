using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.Abstractions;
using Sitecore.DependencyInjection;
using Sitecore.Resources.Media;
using WageWorks.Foundation.Multisite.Placeholders;
using WageWorks.Foundation.Multisite.Providers;

namespace WageWorks.Foundation.Multisite
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.Replace(ServiceDescriptor.Singleton<BasePlaceholderCacheManager, SiteSpecificPlaceholderCacheManager>());

            var linkManagerService = new ServiceDescriptor(typeof(BaseLinkManager), typeof(SwitchingLinkManager), ServiceLifetime.Singleton);
            serviceCollection.Replace(linkManagerService);

            //serviceCollection.AddSingleton<BaseMediaManager>(s =>
            //    new WageWorksMediaProvider(
            //        new DefaultMediaManager(
            //            s.GetService<BaseFactory>(),
            //            s.GetService<MediaProvider>()
            //        )));
        }
    }
}