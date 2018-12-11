using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web;
using Glass.Mapper.Sc.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Wageworks.Foundation.ORM.Context;

namespace Wageworks.Foundation.ORM
{
    public class RegisterDI : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceCollection.AddTransient<ISitecoreContext>(provider => new SitecoreContext());
            serviceCollection.AddTransient<ISitecoreService>(provider => new SitecoreService(Sitecore.Context.ContentDatabase));

            serviceCollection.AddTransient<IRenderingContext, RenderingContextMvcWrapper>();
            serviceCollection.AddTransient<IControllerSitecoreContext, ControllerSitecoreContext>();
            serviceCollection.AddTransient<IGlassHtml, GlassHtml>();
            
        }
    }
}
