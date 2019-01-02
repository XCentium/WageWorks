using WageWorks.Feature.Teasers.Models;
using WageWorks.Feature.Teasers.Models.Glass;

namespace WageWorks.Feature.Teasers.Repositories
{
    public interface ITeaserRepository
    {
        PromoViewModel GetPromo();
        PromoSectionViewModel GetPromoSection();

        IPromotion GetPromoModel();

    }
}