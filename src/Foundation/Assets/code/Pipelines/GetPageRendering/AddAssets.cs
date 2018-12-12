using System.Collections.Generic;
using System.Xml;
using Sitecore.Mvc.Pipelines.Response.GetPageRendering;
using WageWorks.Foundation.Assets.Models;
using WageWorks.Foundation.Assets.Repositories;

namespace WageWorks.Foundation.Assets.Pipelines.GetPageRendering
{
    public class AddAssets : GetPageRenderingProcessor
    {
        private IList<Asset> _siteAssets;

        private IList<Asset> SiteAssets => this._siteAssets ?? (this._siteAssets = new List<Asset>());

        public void AddAsset(XmlNode node)
        {
            var asset = AssetRepository.Current.CreateFromConfiguration(node);
            if (asset != null)
            {
                this.SiteAssets.Add(asset);
            }
        }

        public override void Process(GetPageRenderingArgs args)
        {
            this.AddSiteAssetsFromConfiguration();
        }

        private void AddSiteAssetsFromConfiguration()
        {
            foreach (var asset in this.SiteAssets)
            {
                AssetRepository.Current.Add(asset, true);
            }
        }
    }
}