using Sitecore.Sites;

namespace Wageworks.Foundation.Dictionary.Repositories
{
  public interface IDictionaryRepository
  {
    Models.Dictionary Get(SiteContext site);
  }
}