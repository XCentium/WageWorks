using System.Collections.Generic;

namespace Wageworks.Feature.Teasers.Models
{
    public interface IHasPromoList
    {
        List<PromotionModel> PromoItems { get; set; }
    }
}