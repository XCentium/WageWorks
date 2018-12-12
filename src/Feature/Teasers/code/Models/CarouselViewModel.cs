using System;
using System.Collections.Generic;

namespace WageWorks.Feature.Teasers.Models
{
    public class CarouselViewModel : IHasPromoList
    {
        public string UniqueId { get; set; } = Guid.NewGuid().ToString();
        public List<PromotionModel> PromoItems { get; set; } = new List<PromotionModel>();
        public string CssClass { get; set; }
    }
}