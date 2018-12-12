#region using



#endregion

using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using WageWorks.Foundation.SitecoreExtensions.Extensions;

namespace WageWorks.Foundation.Indexing.Infrastructure.Fields
{
    public class HasPresentationComputedField : IComputedIndexField
  {
    public string FieldName { get; set; }

    public string ReturnType { get; set; }

    public object ComputeFieldValue(IIndexable indexable)
    {
      Item i = indexable as SitecoreIndexableItem;
      if (i.HasLayout())
      {
        return true;
      }
      return null;
    }
  }
}