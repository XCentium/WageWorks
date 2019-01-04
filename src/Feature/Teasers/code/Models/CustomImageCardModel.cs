using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using WageWorks.Foundation.Theming;
using WageWorks.Foundation.SitecoreExtensions.Extensions;

namespace WageWorks.Feature.Teasers.Models
{
    public class CustomImageCardModel :ContentInfoModel
    {
        public static MediaUrlOptions ImageSize { get; set; } = new MediaUrlOptions { Width = 195, Height = 195 };

        public HtmlString Image { get; set; }

        public HtmlString ImageUrl { get; set; }

        public static CustomImageCardModel CreateModel(Database database, Rendering rendering)
        {
            var item = rendering.Item;

            var model = new CustomImageCardModel();
            model.PopulateCustomContentInfoModel(item, null, null, null);

            var imageField = item.GetImageField(Constants.Fields.BackgroundImage);
            if (imageField != null)
            {
                model.Image = new HtmlString(item.Render(Constants.Fields.BackgroundImage, ImageSize.Width, ImageSize.Height));

                var mediaItem = imageField.GetMediaItem();
                if (mediaItem != null)
                {
                    var imageUrl = mediaItem.GetURL(ImageSize);

                    model.ImageUrl = new HtmlString(imageUrl);
                }
            }

            return model;
        }
    }
}