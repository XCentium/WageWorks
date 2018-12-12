﻿using Sitecore.Data.Items;

namespace WageWorks.Foundation.Multisite.Providers
{
    public interface IDatasourceProvider
  {
    Item[] GetDatasourceLocations(Item contextItem, string name);

    Item GetDatasourceTemplate(Item contextItem, string name);
  }
}
