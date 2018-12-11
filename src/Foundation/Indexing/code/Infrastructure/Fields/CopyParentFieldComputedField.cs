using Newtonsoft.Json.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Wageworks.Foundation.Indexing.Infrastructure.Fields
{
    /// <summary>
    /// Read Json field from indexed item and retrieve value(s) using given JsonPath
    /// </summary>
    public class CopyParentFieldComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public string CopyFieldName { get; set; }

        public IList<string> AllowedTemplates { get; set; }


        public CopyParentFieldComputedField(XmlNode configNode) : base()
        {
            this.CopyFieldName = XmlUtil.GetAttribute("copyFieldName", configNode);

            if (string.IsNullOrEmpty(CopyFieldName))
            {
                throw new ArgumentNullException("CopyFieldName", "CopyFieldName parameter is not configured on JsonPathValueComputedField. Check index configuration.");
            }

            var allowedTemplatesString = XmlUtil.GetAttribute("allowedTemplates", configNode);
            if (!string.IsNullOrEmpty(allowedTemplatesString))
            {
                this.AllowedTemplates = allowedTemplatesString.Split(',').ToList();
            }
        }

        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = (Item)(indexable as SitecoreIndexableItem);
            if (item == null)
            {
                return (object)null;
            }

            if (AllowedTemplates != null && !AllowedTemplates.Contains(item.TemplateName, StringComparer.OrdinalIgnoreCase))
            {
                return null;
            }

            var parentItem = item.Parent;

            if (parentItem == null)
            {
                return (object)null;
            }

            var itemField = parentItem.Fields[CopyFieldName];
            if (itemField == null || string.IsNullOrEmpty(itemField.Value))
            {
                return null;
            }

            var fieldValue = Guid.Parse(itemField.Value).ToString("N").ToLower();

            if (string.IsNullOrEmpty(fieldValue))
            { 
                return null;
            }

            return fieldValue;
        }
    }
}