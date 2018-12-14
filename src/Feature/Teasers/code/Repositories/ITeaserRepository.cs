using WageWorks.Feature.Teasers.Models;

namespace WageWorks.Feature.Teasers.Repositories
{
    public interface ITeaserRepository
    {
        PromoViewModel GetPromo();
        PromoSectionViewModel GetPromoSection();

    }
}