using Sitecore;

namespace WageWorks.Feature.News.Infrastructure.Pipelines
{
    using Sitecore.Pipelines;
    using System.Web.Routing;
    using WageWorks.Foundation.DependencyInjection;

    [Service]
    public class InitializeRoutes
    {
        public void Process(PipelineArgs args)
        {
            if (!Context.IsUnitTesting)
            {
                RouteConfig.RegisterRoutes(RouteTable.Routes);
            }
        }
    }
}