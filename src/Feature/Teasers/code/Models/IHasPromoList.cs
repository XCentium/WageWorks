using System.Collections.Generic;

namespace Vista.Feature.Teasers.Models
{
    public interface IHasPromoList
    {
        List<PromotionModel> PromoItems { get; set; }
    }
}