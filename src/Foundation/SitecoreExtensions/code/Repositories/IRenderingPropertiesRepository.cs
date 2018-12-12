using Sitecore.Mvc.Presentation;

namespace WageWorks.Foundation.SitecoreExtensions.Repositories
{
    public interface IRenderingPropertiesRepository
  {
    T Get<T>(Rendering rendering);
  }
}