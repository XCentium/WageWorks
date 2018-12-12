using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;
using System.Web;
using WageWorks.Feature.Teasers.Models;
using WageWorks.Feature.Teasers.Models.Glass;
using WageWorks.Foundation.DependencyInjection;
using WageWorks.Foundation.ORM.Context;
using WageWorks.Foundation.SitecoreExtensions.Extensions;

namespace WageWorks.Feature.Teasers.Repositories
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

        public PromoViewModel GetPromo()
        {
            var vm = new PromoViewModel();

            var promoItem = RenderingContext.Current.Rendering.Item;
            var promoItemModel = _context.Cast<IPromotion>(promoItem);

            var css = RenderingContext.Current.Rendering.Parameters.GetCssClassFromParameters();
            vm.CssClass = css;


            var promoItemViewModel = new PromotionModel(promoItemModel);
            if (promoItemModel.Theme != Guid.Empty)
            {
                var theme = Sitecore.Context.Database.GetItem(new ID(promoItemModel.Theme));
                if (theme != null)
                {
                    promoItemViewModel.CssClass = theme[Foundation.SitecoreExtensions.Constants.PromoLayoutParameters.CssFieldName];
                }
            }

            foreach (Item ctaLink in promoItem.GetChildren())
            {
                try
                {
                    var cta = _context.Cast<ICallToAction>(ctaLink);
                    var ctaModel = new CallToActionLinkModel(cta);
                    ctaModel.Id = ctaLink.ID.Guid.ToString("D");

                    if (ctaLink.TemplateID == Templates.VideoPopupCTA.ID)
                    {
                        ctaModel.IsVideoPopup = true;
                        if (!string.IsNullOrEmpty(ctaLink[Templates.VideoPopupCTA.Fields.Thumbnail]))
                        {
                            ctaModel.BackgroundImage = ctaLink.ImageUrl(Templates.VideoPopupCTA.Fields.Thumbnail);
                        }
                        ctaModel.Text = ctaLink[Templates.VideoPopupCTA.Fields.Description];
                        ctaModel.Title = ctaLink[Templates.VideoPopupCTA.Fields.Title];
                    }

                    promoItemViewModel.Links.Add(ctaModel);
                }
                catch (Exception ex)
                {
                    var msg = ex;
                    // error skip
                }
            }

            vm.PromoItem = promoItemViewModel;
            return vm;
        }



        #endregion

        #region Carousel


        #endregion



        private HttpRequest GetRequest()
        {
            return System.Web.HttpContext.Current.Request;
        }

        //private Item GetContextItem()
        //{
        //    //return WageWorks.Foundation.Commerce.Extensions.CommerceExtensions.GetContextItem(GetRequest());
        //    return null;
        //}
    }
}