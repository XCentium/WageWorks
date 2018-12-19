using Sitecore;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WageWorks.Feature.Teasers.Models
{
    public class ImageCardsModel :BaseComponentModel
    {
        public HtmlString Title { get; set; }

        public IEnumerable<ImageCardModel> Cards { get; set; }

        public static ImageCardsModel CreateModel(string title, string[] itemIds)
        {
            var model = new ImageCardsModel();

            model.PopulateBaseComponent(Context.Database, RenderingContext.Current.Rendering);

            if (!string.IsNullOrEmpty(title))
            {
                model.Title = new HtmlString(title);
            }
            model.Cards = itemIds.Select(x => ImageCardModel.CreateModel(new ID(x)));

            return model;
        }
    }
}