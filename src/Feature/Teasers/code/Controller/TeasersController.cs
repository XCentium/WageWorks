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
    using WageWorks.Foundation.Theming;
    using Constants = Foundation.Theming.Constants;

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
            if (!dataSourceItem?.IsDerived(WageWorks.Feature.Teasers.Templates.DynamicTeaser.ID) ?? true)
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

        public ActionResult ImageCards()
        {
            var title = ItemExtensions.GetParameter(Constants.Fields.Title);
            var itemIds = ItemExtensions.GetParameters(Constants.Fields.Items);

            var model = ImageCardsModel.CreateModel(title, itemIds);

            return View("~/Views/Teasers/ImageCards.cshtml", model);
        }

        public ActionResult PlainCards()
        {
            var title = ItemExtensions.GetParameter(Constants.Fields.Title);
            var itemIds = ItemExtensions.GetParameters(Constants.Fields.Items);
            var openItemText = ItemExtensions.GetParameter(Constants.Fields.OpenItemText);

            var model = PlainCardsModel.CreateModel(title, itemIds, openItemText);

            return View("~/Views/Teasers/PlainCards.cshtml", model);
        }



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

        public ActionResult PromoImage()
        {
            var promoSection = _teaserRepository.GetPromoSection();
            return this.View("PromoImage", promoSection);
        }

        public ActionResult HeadlineSection()
        {
            var promoSection = _teaserRepository.GetPromo();
            return this.View("HeadlineSection", promoSection);
        }

        public ActionResult StorySection()
        {
            var promoSection = _teaserRepository.GetPromo();
            return this.View("StorySection", promoSection);
        }

        public ActionResult RoundSection()
        {
            var promoSection = _teaserRepository.GetPromo();
            return this.View("RoundSection", promoSection);
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