using Newtonsoft.Json.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace WageWorks.Foundation.Indexing.Infrastructure.Fields
{
    /// <summary>
    /// Read Json field from indexed item and retrieve value(s) using given JsonPath
    /// </summary>
    public class CommerceItemWorkflowStateComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public IList<string> AllowedTemplates { get; set; }

        public IList<string> TemplatesToGetWorkflowStateFromParent { get; set; }


        public CommerceItemWorkflowStateComputedField(XmlNode configNode) : base()
        {
            var allowedTemplatesString = XmlUtil.GetAttribute("allowedTemplates", configNode);
            if (!string.IsNullOrEmpty(allowedTemplatesString))
            {
                this.AllowedTemplates = allowedTemplatesString.Split('|').ToList();
            }

            var templatesToGetWorkflowStateFromParent = XmlUtil.GetAttribute("templatesToGetWorkflowStateFromParent", configNode);
            if (!string.IsNullOrEmpty(allowedTemplatesString))
            {
                this.TemplatesToGetWorkflowStateFromParent = templatesToGetWorkflowStateFromParent.Split('|').ToList();
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

            Item itemVersion;
            if (TemplatesToGetWorkflowStateFromParent != null && TemplatesToGetWorkflowStateFromParent.Contains(item.TemplateName, StringComparer.OrdinalIgnoreCase))
            {
                var parentItem = item.Parent;
                if (parentItem == null)
                {
                    return (object)null;
                }

                itemVersion = parentItem.Database?.GetItem(parentItem.ID, item.Language, Sitecore.Data.Version.Parse(item.Version.Number));
            }
            else
            {
                itemVersion = item;
            }

            if (itemVersion == null)
            {
                return (object)null;
            }

            var itemField = itemVersion.Fields["__Workflow state"];
            if (itemField == null || string.IsNullOrEmpty(itemField.Value))
            {
                return null;
            }

            Guid guidValue;
            if (Guid.TryParse(itemField.Value, out guidValue))
            {
                return guidValue.ToString("N").ToLower();
            }
            else
            {
                return itemField.Value;
            }
        }
    }
}