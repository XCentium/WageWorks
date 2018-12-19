using Sitecore.Data;
using Sitecore.Data.Items;
using System.Web;
using WageWorks.Foundation.SitecoreExtensions.Extensions;
using WageWorks.Foundation.Theming;

namespace WageWorks.Feature.Teasers.Models
{
    public class LinkModel : BaseComponentModel
    {
        public HtmlString Text { get; set; }
        public HtmlString Url { get; set; }
        public HtmlString IconClass { get; set; }
        public HtmlString LinkClass { get; set; }
        public bool IsBoldText { get; set; }

        public static LinkModel CreateModelWithIconThemeIdAndLinkThemeId(ID linkId, ID iconThemeId, ID linkThemeId)
        {
            var model = CreateModel(linkId);

            model.IconClass = new HtmlString(ThemeModel.GetClass(iconThemeId));
            model.LinkClass = new HtmlString(ThemeModel.GetClass(linkThemeId));

            return model;
        }

        public static LinkModel CreateModelWithIconThemeId(ID linkId, ID iconThemeId)
        {
            var model = CreateModel(linkId);

            model.IconClass = new HtmlString(ThemeModel.GetClass(iconThemeId));

            return model;
        }

        public static LinkModel CreateModelWithLinkThemeId(ID linkId, ID linkThemeId)
        {
            var model = CreateModel(linkId);

            model.LinkClass = new HtmlString(ThemeModel.GetClass(linkThemeId));

            return model;
        }

        public static LinkModel CreateModelWithLinkClass(ID linkId, string linkClass)
        {
            var model = CreateModel(linkId);

            if (!string.IsNullOrEmpty(linkClass))
            {
                model.LinkClass = new HtmlString(linkClass);
            }

            return model;
        }

        public static LinkModel CreateModel(ID linkId)
        {
            var link = Sitecore.Context.Database.Items[linkId];

            if (link.TemplateID == new ID(Constants.Datasources.Link))
            {
                return new LinkModel()
                {
                    Text = new HtmlString(link.Render(Constants.ComponentParameters.Text)),
                    Url = new HtmlString(link.Render(Constants.ComponentParameters.Url)),
                };
            }
            else if (link.TemplateID == new ID(Constants.Datasources.IconLink))
            {
                return new LinkModel()
                {
                    IconClass = new HtmlString(ThemeModel.GetClass(new ID(link.Fields[Constants.ComponentParameters.IconTheme].Value))),
                    Url = new HtmlString(link.Render(Constants.ComponentParameters.Url)),
                };
            }

            return new LinkModel();
        }

        public static LinkModel CreateModelFromItem(ID itemId)
        {
            var item = Sitecore.Context.Database.GetItem(itemId);
            var model = new LinkModel
            {
                Text = new HtmlString(item.RenderDisableEdit(Constants.ComponentParameters.Name)),
                Url = new HtmlString(item.Url()),
            };

            return model;
        }


        public static LinkModel CreateModel(string text, string url)
        {
            var model = new LinkModel
            {
                Text = new HtmlString(text),
                Url = new HtmlString(url),
            };

            return model;
        }

        public static LinkModel CreateModel(string text, string url, string linkClass)
        {
            var model = new LinkModel
            {
                Text = new HtmlString(text),
                Url = new HtmlString(url),
                LinkClass = new HtmlString(linkClass),
            };

            return model;
        }

        public static LinkModel CreateModel(Item item, HtmlString linkText, HtmlString iconClass, HtmlString linkClass)
        {
            return new LinkModel()
            {
                Url = new HtmlString(item.Url()),
                LinkClass = linkClass,
                Text = linkText,
            };
        }

    }
}