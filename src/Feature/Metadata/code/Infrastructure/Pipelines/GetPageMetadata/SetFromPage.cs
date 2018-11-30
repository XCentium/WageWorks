using Wageworks.Foundation.Commerce;
using Wageworks.Foundation.Commerce.Extensions;

namespace Wageworks.Feature.Metadata.Infrastructure.Pipelines.GetPageMetadata
{
    using Sitecore.Data.Fields;
    using Sitecore.Data.Items;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Wageworks.Feature.Metadata.Models;
    using Wageworks.Foundation.DependencyInjection;
    using Wageworks.Foundation.SitecoreExtensions.Extensions;

    [Service]
    public class SetFromPage
    {
        public void Process(GetPageMetadataArgs args)
        {
            if (!args.Item.IsDerived(Templates.PageMetadata.ID))
                return;

            this.SetKeywords(args.Item, args.Metadata.KeywordsList);
            this.SetPageTitle(args.Item, args.Metadata);
            this.SetDescription(args.Item, args.Metadata);

            if (Sitecore.Context.Site != null)
            {
                this.SetIndexingFlags(args.Item, args.Metadata.Robots);
            }

            this.SetCustomMetadata(args.Item, args.Metadata.CustomMetadata);
        }

        private void SetCustomMetadata(Item item, ICollection<KeyValuePair<string, string>> customMetadata)
        {
            if (!item.FieldHasValue(Templates.PageMetadata.Fields.CustomMetadata))
                return;
            var values = ((NameValueListField)item.Fields[Templates.PageMetadata.Fields.CustomMetadata])?.NameValues;
            if (values == null)
                return;
            foreach (var key in values.AllKeys)
            {
                var value = HttpUtility.UrlDecode(values[key]);
                customMetadata.Add(new KeyValuePair<string, string>(key, value));
            }
        }

        private void SetDescription(Item item, IMetadata metadata)
        {
            string description;
            if (item.IsProduct() || item.IsProductVariant())
            {
                description = item[Wageworks.Foundation.Commerce.Templates.Product.Fields.ProductGroupOverview];
            }
            else
            {
                var test = Wageworks.Foundation.Commerce.Extensions.CommerceExtensions.GetContextItem(System.Web.HttpContext.Current.Request);
                if (test != null && (test.IsProduct() || test.IsProductVariant()))
                {
                    description = test[Wageworks.Foundation.Commerce.Templates.Product.Fields.ProductGroupOverview];
                }
                else
                {
                    description = item[Templates.PageMetadata.Fields.Description];
                }
            }

            if (string.IsNullOrWhiteSpace(description))
                return;
            metadata.Description = description;
        }

        private void SetPageTitle(Item item, IMetadata metadata)
        {

            var title = string.Empty;

            var test = Wageworks.Foundation.Commerce.Extensions.CommerceExtensions.GetContextItem(System.Web.HttpContext.Current.Request);
            if (test != null && (test.IsProduct() || test.IsProductVariant()))
            {
                title = CommerceExtensions.GetProductTitle(test);
                if (test.IsProduct())
                {
                    title += " - " + CommerceExtensions.GetProductFieldValue(test, Constants.JsonFields.ProductFields.ProductGroupId);
                }
                else if (test.IsProductVariant())
                {
                    title += " - " + CommerceExtensions.GetSkuFieldValue(test, Constants.JsonFields.SkuFields.SkuNumber);
                }
            }
            else
            {
                title = item[Templates.PageMetadata.Fields.BrowserTitle];
            }

            if (string.IsNullOrWhiteSpace(title))
                return;
            metadata.PageTitle = title;
        }

        private void SetIndexingFlags(Item item, ICollection<string> robotsMetadata)
        {
            if (!(item.Fields[Templates.PageMetadata.Fields.CanIndex]?.IsChecked() ?? true))
            {
                robotsMetadata.Add("NOINDEX");
            }
            if (!(item.Fields[Templates.PageMetadata.Fields.CanFollow]?.IsChecked() ?? true))
            {
                robotsMetadata.Add("NOFOLLOW");
            }
        }

        private void SetKeywords(Item item, ICollection<string> keywordsList)
        {
            var keywordsField = item.Fields[Templates.PageMetadata.Fields.Keywords];
            if (keywordsField == null)
                return;

            var keywordMultilist = new MultilistField(keywordsField);
            foreach (var keyword in keywordMultilist.GetItems().Select(i => i[Templates.Keyword.Fields.Keyword]))
            {
                keywordsList.Add(keyword);
            }
        }
    }
}