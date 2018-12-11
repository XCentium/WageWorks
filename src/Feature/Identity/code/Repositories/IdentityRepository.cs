using Sitecore;
using Wageworks.Foundation.SitecoreExtensions.Extensions;

namespace Wageworks.Feature.Identity.Repositories
{
  using System;
  using Sitecore.Data.Items;
  using Wageworks.Foundation.SitecoreExtensions.Extensions;

  public static class IdentityRepository
  {
    public static Item Get([NotNull] Item contextItem)
    {
      if (contextItem == null)
        throw new ArgumentNullException(nameof(contextItem));

      return contextItem.GetAncestorOrSelfOfTemplate(Templates.Identity.ID) ?? Context.Site.GetContextItem(Templates.Identity.ID);
    }
  }
}