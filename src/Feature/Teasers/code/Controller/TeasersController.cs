using Sitecore;
using Sitecore.Mvc.Controllers;
using Vista.Feature.Teasers.Repositories;
using Vista.Foundation.ORM.Context;

namespace Vista.Feature.Teasers.Controller
{
    using Sitecore.Mvc.Presentation;
    using System.Web.Mvc;
    using Vista.Feature.Teasers.Models;
    using Vista.Foundation.Alerts;
    using Vista.Foundation.Alerts.Extensions;
    using Vista.Foundation.Alerts.Models;
    using Vista.Foundation.SitecoreExtensions.Extensions;

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

        public ActionResult Carousel()
        {
            var carousel = _teaserRepository.GetCarousel();
            return View(carousel);
        }

        public ActionResult Promo()
        {
            var carousel = _teaserRepository.GetPromo();
            return View(carousel);
        }
        public ActionResult ProductPromoSection() {
            var productPromo = _teaserRepository.GetProductPromo();
            return View(productPromo);
        }
        public ActionResult PromoSection()
        {
            var promoSection = _teaserRepository.GetPromoSection();
            return View(promoSection);
        }

        public ActionResult PromoGrid()
        {
            var promoSection = _teaserRepository.GetPromoSection();
            return View(promoSection);
        }

        public ActionResult PromoSectionSlider()
        {
            var promoSection = _teaserRepository.GetPromoSection();
            return View(promoSection);
        }

        public ActionResult PromoSwitcher()
        {
            var promoSection = _teaserRepository.GetPromoSection();
            return View(promoSection);
        }

        public ActionResult SocialPromoSection()
        {
            var promoSection = _teaserRepository.GetPromoSection();
            return View(promoSection);
        }

    }
}