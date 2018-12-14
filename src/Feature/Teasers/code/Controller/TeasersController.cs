using Sitecore;
using Sitecore.Mvc.Controllers;
using WageWorks.Feature.Teasers.Repositories;
using WageWorks.Foundation.ORM.Context;

namespace WageWorks.Feature.Teasers.Controller
{
    using Sitecore.Mvc.Presentation;
    using System.Web.Mvc;
    using WageWorks.Feature.Teasers.Models;
    using WageWorks.Foundation.Alerts;
    using WageWorks.Foundation.Alerts.Extensions;
    using WageWorks.Foundation.Alerts.Models;
    using WageWorks.Foundation.SitecoreExtensions.Extensions;

    public class TeasersController : SitecoreController
    {
        private readonly ITeaserRepository _teaserRepository;
        private readonly IControllerSitecoreContext _scContext;

        public TeasersController(ITeaserRepository teaserRepository, IControllerSitecoreContext scContext)
        {
            this._teaserRepository = teaserRepository;
            this._scContext = scContext;
        }

        public ActionResult GetDynamicContent(string viewName)
        {
            var dataSourceItem = RenderingContext.Current.Rendering.Item;
            if (!dataSourceItem?.IsDerived(Templates.DynamicTeaser.ID) ?? true)
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(AlertTexts.InvalidDataSourceTemplateFriendlyMessage, InfoMessage.MessageType.Error)) : null;
            }

            var model = new DynamicTeaserModel(dataSourceItem);
            return this.View(viewName, model);
        }

        public ActionResult Accordion() => this.GetDynamicContent("Accordion");

        public ActionResult Tabs() => this.GetDynamicContent("Tabs");

        public ActionResult TeaserCarousel() => this.GetDynamicContent("TeaserCarousel");

        public ActionResult JumbotronCarousel() => this.GetDynamicContent("JumbotronCarousel");

        //public ActionResult Carousel()
        //{
        //    var carousel = _teaserRepository.GetCarousel();
        //    return View(carousel);
        //}

        public ActionResult Promo()
        {
            var carousel = _teaserRepository.GetPromo();
            return View(carousel);
        }
        //public ActionResult ProductPromoSection() {
        //    var productPromo = _teaserRepository.GetProductPromo();
        //    return View(productPromo);
        //}
        //public ActionResult PromoSection()
        //{
        //    var promoSection = _teaserRepository.GetPromoSection();
        //    return View(promoSection);
        //}

        //public ActionResult PromoGrid()
        //{
        //    var promoSection = _teaserRepository.GetPromoSection();
        //    return View(promoSection);
        //}

        public ActionResult PromoSectionSlider()
        {
            var promoSection = _teaserRepository.GetPromoSection();
            return View(promoSection);
        }

        public ActionResult GuideSelector()
        {
            var promoSection = _teaserRepository.GetPromoSection();
            return this.View("PromoSectionSlider", promoSection);
        }

        //public ActionResult PromoSwitcher()
        //{
        //    var promoSection = _teaserRepository.GetPromoSection();
        //    return View(promoSection);
        //}

        //public ActionResult SocialPromoSection()
        //{
        //    var promoSection = _teaserRepository.GetPromoSection();
        //    return View(promoSection);
        //}

    }
}