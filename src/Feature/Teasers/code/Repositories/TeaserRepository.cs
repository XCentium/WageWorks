using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;
using System.Web;
using Wageworks.Feature.Teasers.Models;
using Wageworks.Feature.Teasers.Models.Glass;
using Wageworks.Foundation.DependencyInjection;
using Wageworks.Foundation.ORM.Context;
using Wageworks.Foundation.SitecoreExtensions.Extensions;

namespace Wageworks.Feature.Teasers.Repositories
{
    [Service(typeof(ITeaserRepository), Lifetime = Lifetime.Transient)]
    public class TeaserRepository : ITeaserRepository
    {
        private readonly IControllerSitecoreContext _context;

        public TeaserRepository(IControllerSitecoreContext context)
        {
            this._context = context;
        }

        #region Promos





        #endregion

        #region Carousel


        #endregion

       

        private HttpRequest GetRequest()
        {
            return System.Web.HttpContext.Current.Request;
        }

        //private Item GetContextItem()
        //{
        //    //return Wageworks.Foundation.Commerce.Extensions.CommerceExtensions.GetContextItem(GetRequest());
        //    return null;
        //}
    }
}