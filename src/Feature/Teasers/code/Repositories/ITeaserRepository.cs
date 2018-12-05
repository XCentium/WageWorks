using Vista.Feature.Teasers.Models;

namespace Vista.Feature.Teasers.Repositories
{
    public interface ITeaserRepository
    {
        CarouselViewModel GetCarousel();
        PromoViewModel GetPromo();
        PromoSectionViewModel GetPromoSection();
        PromoSectionViewModel GetProductPromo();
    }
}