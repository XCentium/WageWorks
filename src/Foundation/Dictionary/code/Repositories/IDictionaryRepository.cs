using Sitecore.Sites;

namespace WageWorks.Foundation.Dictionary.Repositories
{
  public interface IDictionaryRepository
  {
    Models.Dictionary Get(SiteContext site);
  }
}