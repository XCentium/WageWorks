using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Diagnostics;
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
    public class JsonPathMappedValuesComputedField : JsonPathComputedFieldBase, IComputedIndexField
    {
        public string MappedJsonFieldName { get; set; }
        public Dictionary<string, string> Mappings;
        public JsonPathMappedValuesComputedField(XmlNode configNode) : base(configNode)
        {
            this.MappedJsonFieldName = XmlUtil.GetAttribute("mappedJsonFieldName", configNode);
            if (string.IsNullOrEmpty(MappedJsonFieldName))
            {
                throw new ArgumentNullException("MappedJsonFieldName", "MappedJsonFieldName parameter is not configured on JsonPathMappedValuesComputedField. Check index configuration.");
            }

            var fieldMappings = XmlUtil.GetAttribute("fieldMappings", configNode);
            if (string.IsNullOrEmpty(MappedJsonFieldName))
            {
                throw new ArgumentNullException("FieldMappings", "FieldMappings parameter is not configured on JsonPathMappedValuesComputedField. Check index configuration.");
            }

            Mappings = new Dictionary<string, string>();
            foreach (var mappingNode in fieldMappings.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var mapping = mappingNode.Split('|');
                if (mapping.Count() != 2)
                {
                    throw new ArgumentNullException("FieldMappings", "Incorrect FieldMappings configuration on JsonPathMappedValuesComputedField, shoudl follow this pattern: 'label1:jsonPath1|label2:jsonPath2...'. Check index configuration.");
                }
                Mappings.Add(mapping[0], mapping[1]);
            }
        }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var item = base.GetItem(indexable);
            if (item == null)
            {
                return null;
            }
            var parentItem = item.Parent;
            if (parentItem == null)
            {
                return null;
            }

            try
            {
                var label = base.GetTokenValue(parentItem, base.JsonFieldName, base.JsonPath) as string;
                if (label == null || !Mappings.ContainsKey(label))
                {
                    return null;
                }

                var variantValue = base.GetTokenValue(item, MappedJsonFieldName, Mappings[label]);
                return !string.IsNullOrEmpty(variantValue.ToString()) ? string.Format("{0}|{1}", label, variantValue.ToString()) : null;
            }
            catch (Exception ex)
            {
                Log.Warn($"Could not retrieve attribute for compute field: Field Name: {this.FieldName}, Item Id: {item.ID}, Item Name: {item.Name}", ex, this);
                return null;
            }
        }
    }
}