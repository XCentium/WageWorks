using Sitecore.Data;
using Sitecore.Resources.Media;
using System.Web;
using WageWorks.Foundation.SitecoreExtensions.Extensions;
using WageWorks.Foundation.Theming;

namespace WageWorks.Feature.Teasers.Models
{
    public class ImageCardModel : ContentInfoModel
    {
        public static MediaUrlOptions ImageSize { get; set; } = new MediaUrlOptions { Width = 350, Height = 160, IgnoreAspectRatio = true };

        public HtmlString Image { get; set; }

        public HtmlString ImageUrl { get; set; }

        public HtmlString ImageCss { get; set; }

        public static ImageCardModel CreateModel(ID itemId)
        {
            var item = Sitecore.Context.Database.GetItem(itemId);

            var model = new ImageCardModel();
            model.PopulateContentInfoModel(item, null, null, null);

            var imageField = item.GetImageField(Constants.Fields.Image);
            var calculatedImageSize = ImageSize;
            if (imageField.Width == imageField.Height)
            {
                calculatedImageSize = new MediaUrlOptions { Height = ImageSize.Height };
            }

            model.Image = new HtmlString(item.Render(Constants.Fields.Image, calculatedImageSize.Width, calculatedImageSize.Height));

            var mediaItem = imageField.GetMediaItem();
            if (mediaItem != null)
            {
                var imageUrl = mediaItem.GetURL(calculatedImageSize);
                model.ImageUrl = new HtmlString(imageUrl);
            }

            var cardThemeField = item.Fields[Constants.Fields.CardColor];
            var cardThemeId = cardThemeField.Value;

            if (!string.IsNullOrEmpty(cardThemeId))
            {
                model.ImageCss = new HtmlString(ThemeModel.GetClass(new ID(cardThemeId)));
            }

            return model;
        }
    }

}