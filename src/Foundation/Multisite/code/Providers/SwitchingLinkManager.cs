using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Abstractions;
using Sitecore.Configuration;
using Sitecore.Links;

namespace Wageworks.Foundation.Multisite.Providers
{
    public class SwitchingLinkManager : Sitecore.Links.DefaultLinkManager
    {
        private readonly ProviderHelper<LinkProvider, LinkProviderCollection> providerHelper;

        protected virtual LinkProvider Provider
        {
            get
            {
                var siteLinkProvider = (Sitecore.Context.Site != null)
                    ? Sitecore.Context.Site.Properties["linkProvider"] : String.Empty;

                if (String.IsNullOrEmpty(siteLinkProvider))
                    return this.providerHelper.Provider;

                return this.providerHelper.Providers[siteLinkProvider]
                       ?? this.providerHelper.Provider;
            }
        }

        public SwitchingLinkManager(ProviderHelper<LinkProvider, LinkProviderCollection> providerHelper) : base(providerHelper)
        {
            this.providerHelper = providerHelper;
        }
    }
}