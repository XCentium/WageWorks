using Sitecore.Mvc.Presentation;
using System.Collections.Generic;

namespace Wageworks.Feature.Teasers.Models
{
    public class PromoSectionViewModel : RenderingModel, IHasPromoList
    {
        public string CssClass { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<PromotionModel> PromoItems { get; set; } = new List<PromotionModel>();
        public CallToActionLinkModel MoreLink { get; set; }
        public string BackgroundImage { get; set; }
    }
}