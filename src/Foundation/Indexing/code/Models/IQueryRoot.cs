using Sitecore.Data.Items;

namespace Wageworks.Foundation.Indexing.Models
{
    public interface IQueryRoot
    {
        Item Root { get; set; }
    }
}