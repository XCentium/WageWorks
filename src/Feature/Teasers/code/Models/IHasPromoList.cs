using System.Collections.Generic;

namespace WageWorks.Feature.Teasers.Models
{
    public interface IHasPromoList
    {
        List<PromotionModel> PromoItems { get; set; }
    }
}