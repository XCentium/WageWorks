using Sitecore.Mvc.Pipelines.Response.GetPageRendering;
using Wageworks.Foundation.Assets.Repositories;

namespace Wageworks.Foundation.Assets.Pipelines.GetPageRendering
{
    public class ClearAssets : GetPageRenderingProcessor
    {
        public override void Process(GetPageRenderingArgs args)
        {
            AssetRepository.Current.Clear();
        }
    }
}