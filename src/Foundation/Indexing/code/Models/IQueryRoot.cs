using Sitecore.Data.Items;

namespace WageWorks.Foundation.Indexing.Models
{
    public interface IQueryRoot
    {
        Item Root { get; set; }
    }
}