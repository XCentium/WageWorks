using Sitecore.Data;
using Sitecore.Data.Items;
using System.Web;
using WageWorks.Foundation.SitecoreExtensions.Extensions;
using WageWorks.Foundation.Theming;
using WebControls = Sitecore.Web.UI.WebControls;

namespace WageWorks.Feature.Teasers.Models
{
    public class ContentInfoModel : BaseComponentModel
    {
        public HtmlString ContentType { get; set; }

        public HtmlString Name { get; set; }

        public HtmlString Title { get; set; }

        public HtmlString Headline { get; set; }

        public HtmlString Description { get; set; }

        public LinkModel Link { get; set; }

        public static HtmlString GetContentType(Item item)
        {
            if (item.TemplateID == new ID(Constants.Datasources.Guide))
                return new HtmlString("Guide");
            else if (item.TemplateID == new ID(Constants.Datasources.HowTo))
                return new HtmlString("How-To");
            else if (item.TemplateID == new ID(Constants.Datasources.Tool))
                return new HtmlString("Resource / Tool");
            else if (item.TemplateID == new ID(Constants.Datasources.Article))
                return new HtmlString("Resource / Article");

            return new HtmlString(string.Empty);
        }

        protected void PopulateContentInfoModel(Item item, HtmlString linkText, HtmlString iconClass, HtmlString linkClass)
        {
            ContentType = GetContentType(item);
            Name = new HtmlString(item.Render(Constants.Fields.Name));
            Title = new HtmlString(item.Render(Constants.Fields.Title));
            Headline = new HtmlString(item.Render(Constants.Fields.Headline));
            Description = new HtmlString(item.Render(Constants.Fields.Description));
            Link = LinkModel.CreateModel(item, linkText, iconClass, linkClass);
        }

        //public string Render(this Item item, string fieldName)
        //{
        //    return WebControls.FieldRenderer.Render(item, fieldName);
        //}


    }
}