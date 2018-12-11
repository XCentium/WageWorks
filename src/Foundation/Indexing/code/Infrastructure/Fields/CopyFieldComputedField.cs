using Newtonsoft.Json.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Xml;
using System;
using System.Linq;
using System.Xml;

namespace Wageworks.Foundation.Indexing.Infrastructure.Fields
{
    /// <summary>
    /// Read Json field from indexed item and retrieve value(s) using given JsonPath
    /// </summary>
    public class CopyFieldComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public string CopyFieldName { get; set; }

        public CopyFieldComputedField(XmlNode configNode) : base()
        {
            this.CopyFieldName = XmlUtil.GetAttribute("copyFieldName", configNode);

            if (string.IsNullOrEmpty(CopyFieldName))
            {
                throw new ArgumentNullException("CopyFieldName", "CopyFieldName parameter is not configured on JsonPathValueComputedField. Check index configuration.");
            }
        }

        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = (Item)(indexable as SitecoreIndexableItem);
            if (item == null)
            {
                return (object)null;
            }

            var itemField = item.Fields[CopyFieldName];
            if (itemField == null)
            {
                return null;
            }

            var fieldValue = itemField.Value;

            if (string.IsNullOrEmpty(fieldValue))
            { 
                return null;
            }

            return fieldValue;
        }
    }
}