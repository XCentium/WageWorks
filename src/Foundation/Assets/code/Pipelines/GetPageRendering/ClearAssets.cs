using Sitecore.Mvc.Pipelines.Response.GetPageRendering;
using WageWorks.Foundation.Assets.Repositories;

namespace WageWorks.Foundation.Assets.Pipelines.GetPageRendering
{
    public class ClearAssets : GetPageRenderingProcessor
    {
        public override void Process(GetPageRenderingArgs args)
        {
            AssetRepository.Current.Clear();
        }
    }
}