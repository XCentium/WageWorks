using Wageworks.Feature.Teasers.Models;

namespace Wageworks.Feature.Teasers.Repositories
{
    public interface ITeaserRepository
    {
        CarouselViewModel GetCarousel();
        PromoViewModel GetPromo();
        PromoSectionViewModel GetPromoSection();
        PromoSectionViewModel GetProductPromo();
    }
}