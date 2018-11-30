using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sitecore.Abstractions;
using Sitecore.DependencyInjection;
using Sitecore.Resources.Media;
using Wageworks.Foundation.Multisite.Placeholders;
using Wageworks.Foundation.Multisite.Providers;

namespace Wageworks.Foundation.Multisite
{
    public class ServicesConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.Replace(ServiceDescriptor.Singleton<BasePlaceholderCacheManager, SiteSpecificPlaceholderCacheManager>());

            var linkManagerService = new ServiceDescriptor(typeof(BaseLinkManager), typeof(SwitchingLinkManager), ServiceLifetime.Singleton);
            serviceCollection.Replace(linkManagerService);

            //serviceCollection.AddSingleton<BaseMediaManager>(s =>
            //    new WageworksMediaProvider(
            //        new DefaultMediaManager(
            //            s.GetService<BaseFactory>(),
            //            s.GetService<MediaProvider>()
            //        )));
        }
    }
}