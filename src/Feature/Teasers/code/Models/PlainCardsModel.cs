using Sitecore;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WageWorks.Feature.Teasers.Models
{
    public class PlainCardsModel : BaseComponentModel
    {
        public HtmlString Title { get; set; }
        public IEnumerable<PlainCardModel> Cards { get; set; }
        public HtmlString OpenItemText { get; set; }


        public static PlainCardsModel CreateModel(string title, string[] itemIds, string openItemText)
        {
            var model = new PlainCardsModel();

            model.PopulateBaseComponent(Context.Database, RenderingContext.Current.Rendering);

            if (!string.IsNullOrEmpty(title))
            {
                model.Title = new HtmlString(title);
            }

            model.Cards = itemIds.Select(x => PlainCardModel.CreateModel(new ID(x)));

            if (!string.IsNullOrEmpty(openItemText))
            {
                model.OpenItemText = new HtmlString(openItemText);
            }

            return model;
        }
    }
}