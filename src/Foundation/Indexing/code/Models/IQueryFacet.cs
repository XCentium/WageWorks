namespace Wageworks.Foundation.Indexing.Models
{
    public interface IQueryFacet
    {
        string Title { get; set; }
        string FieldName { get; set; }
        string ViewName { get; set; }
        int MinimumFacetCount { get; set; }
    }
}