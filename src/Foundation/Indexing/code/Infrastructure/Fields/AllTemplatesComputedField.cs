#region using



#endregion

using System.Collections.Generic;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Wageworks.Foundation.Indexing.Infrastructure.Fields
{
    public class AllTemplatesComputedField : IComputedIndexField
  {
    public string FieldName { get; set; }
    public string ReturnType { get; set; }

    public object ComputeFieldValue(IIndexable indexable)
    {
            var indexItem = indexable as SitecoreIndexableItem;
            if (indexItem == null)
            {
                return null;
            }
            var item = indexItem.Item;
            try
            {
                var templates = new List<string>();
                this.GetAllTemplates(item.Template, templates);
                return templates;
            }
            catch (System.Exception ex)
            {
                Log.Warn($"Could not retrieve attribute for compute field: Field Name: {this.FieldName}, Item Id: {item.ID}, Item Name: {item.Name},", ex, this);
                return null;
            }
    }

    public void GetAllTemplates(TemplateItem baseTemplate, List<string> list)
    {
      var str = IdHelper.NormalizeGuid(baseTemplate.ID);
      list.Add(str);
      if (baseTemplate.ID == TemplateIDs.StandardTemplate)
      {
        return;
      }
      foreach (var item in baseTemplate.BaseTemplates)
      {
        this.GetAllTemplates(item, list);
      }
    }
  }
}