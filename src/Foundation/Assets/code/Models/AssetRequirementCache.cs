using Sitecore.Caching;
using Sitecore.Data;

namespace WageWorks.Foundation.Assets.Models
{
    internal class AssetRequirementCache : CustomCache
  {
    public AssetRequirementCache(long maxSize) : base("WageWorks.Foundation.AssetRequirements", maxSize)
    {
    }

    public AssetRequirementList Get(string cacheKey)
    {
      return (AssetRequirementList)this.GetObject(cacheKey.ToString());
    }

    public void Set(string cacheKey, AssetRequirementList requirementList)
    {
      this.SetObject(cacheKey.ToString(), requirementList);
    }
  }
}