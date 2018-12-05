using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;
using System.Web;
using Vista.Feature.Teasers.Models;
using Vista.Feature.Teasers.Models.Glass;
using Vista.Foundation.DependencyInjection;
using Vista.Foundation.ORM.Context;
using Vista.Foundation.SitecoreExtensions.Extensions;

namespace Vista.Feature.Teasers.Repositories
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

        public PromoSectionViewModel GetProductPromo()
        {
            var vm = new PromoSectionViewModel();

            var parentItem = _context.Cast<IPromoSection>(RenderingContext.Current.Rendering.Item);
            if (parentItem == null) return vm;

            vm.Description = parentItem.Description;

            var css = RenderingContext.Current.Rendering.Parameters.GetCssClassFromParameters();
            vm.CssClass = css;

            vm.MoreLink = new CallToActionLinkModel()
            {
                Text = parentItem.Link?.Text,
                Url = parentItem.Link?.Url,
                Target = parentItem.Link?.Target,
                CssClass = parentItem.Link?.Class
            };

            vm.Title = parentItem.Name;

            PopulatePromos(vm);

            return vm;
        }



        public PromoSectionViewModel GetPromoSection()
        {
            var vm = new PromoSectionViewModel();

            var parentItem = _context.Cast<IPromoSection>(RenderingContext.Current.Rendering.Item);
            if (parentItem == null) return vm;

            vm.Description = parentItem.Description;
            vm.BackgroundImage = parentItem.BackgroundImage?.Src;

            var css = RenderingContext.Current.Rendering.Parameters.GetCssClassFromParameters();
            vm.CssClass = css;

            vm.MoreLink = new CallToActionLinkModel()
            {
                Text = parentItem.Link?.Text,
                Url = parentItem.Link?.Url,
                Target = parentItem.Link?.Target,
                CssClass = parentItem.Link?.Class
            };

            vm.Title = parentItem.Name;

            PopulatePromos(vm);

            return vm;
        }
        #endregion

        #region Carousel

        public CarouselViewModel GetCarousel()
        {
            var vm = new CarouselViewModel();

            var parentItem = RenderingContext.Current.Rendering.Item;
            if (parentItem == null) return vm;

            vm.UniqueId = parentItem.ID.Guid.ToString();

            PopulatePromos(vm);

            return vm;
        }

        #endregion

        private void PopulatePromos(IHasPromoList vm)
        {
            foreach (Item promo in RenderingContext.Current.Rendering.Item.GetChildren())
            {
                if (promo.IsDerived(Templates.RelatedProductPromo.ID))
                {
                    // if it's meant to be global, it shouldn't be using this template
                    if (string.IsNullOrEmpty(promo[Templates.RelatedProductPromo.Fields.RelatedProducts])) continue;

                    var product = GetContextItem();
                    if (product == null) continue;

                    var ids = promo[Templates.RelatedProductPromo.Fields.RelatedProducts].ToLower();
                    if (product == null || !ids.Contains(product.ID.Guid.ToString().ToLower())) continue;
                }

                if (promo.IsDerived(Templates.ConditionalPromo.ID))
                {

                    var sourceField = promo[Templates.ConditionalPromo.Fields.SourceField];
                    if (string.IsNullOrEmpty(sourceField)) continue;

                    var sourceItem = Sitecore.Context.Database.GetItem(new ID(new Guid(sourceField)));
                    if (sourceItem == null) continue;

                    var sourceFieldPath = sourceItem[Templates.Constants.JsonFields.JsonPathField];
                    if (string.IsNullOrEmpty(sourceFieldPath)) continue;

                    var product = GetContextItem();
                    if (product == null) continue;

                    if (!product.IsDerived(Templates.ProductVariant.ID))
                        product = product.Children.FirstOrDefault();

                    if (product == null) continue;

                    var conditionalValue = JsonExtensions.GetTokenValues(Sitecore.Context.Database.GetItem(product.ID), Templates.Constants.JsonFields.SkuJsonField, sourceFieldPath)?.FirstOrDefault();
                    if (conditionalValue == null) continue;

                    var expectedValue = promo[Templates.ConditionalPromo.Fields.ExpectedValue];
                    if (!conditionalValue.Equals(expectedValue, StringComparison.OrdinalIgnoreCase)) continue;

                }


                var slideSitecoreModel = _context.Cast<IPromotion>(promo);
                var slideModel = new PromotionModel(slideSitecoreModel);

                // append theme for promo
                if (slideSitecoreModel.Theme != Guid.Empty)
                {
                    var theme = Sitecore.Context.Database.GetItem(new ID(slideSitecoreModel.Theme));
                    if (theme != null)
                    {
                        slideModel.CssClass = theme[Foundation.SitecoreExtensions.Constants.PromoLayoutParameters.CssFieldName];
                    }
                }

                // append css class for carousel slides
                if (promo.IsDerived(Templates.Slide.ID) && !string.IsNullOrEmpty(promo[Templates.Slide.Fields.SlideStyle]))
                {
                    var style = Sitecore.Context.Database.GetItem(new ID(promo[Templates.Slide.Fields.SlideStyle]));
                    if (style != null)
                    {
                        slideModel.CssClass += $" {style[Foundation.SitecoreExtensions.Constants.PromoLayoutParameters.CssFieldName]}";
                    }
                }

                foreach (Item ctaLink in promo.GetChildren())
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

                        slideModel.Links.Add(ctaModel);
                    }
                    catch (Exception ex)
                    {
                        var log = ex; // TODO: log exception
                    }
                }

                vm.PromoItems.Add(slideModel);
            }
        }

        private HttpRequest GetRequest()
        {
            return System.Web.HttpContext.Current.Request;
        }

        private Item GetContextItem()
        {
            return Vista.Foundation.Commerce.Extensions.CommerceExtensions.GetContextItem(GetRequest());
        }
    }
}