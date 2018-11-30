namespace Wageworks.Feature.Metadata.Infrastructure.Pipelines.GetPageMetadata
{
    using Wageworks.Foundation.DependencyInjection;

    [Service]
    public class CombineTitle
    {
        public void Process(GetPageMetadataArgs args)
        {
            args.Metadata.Title = args.Metadata.SiteTitle;
            if (!string.IsNullOrEmpty(args.Metadata.PageTitle))
                args.Metadata.Title += $" - {args.Metadata.PageTitle}";
        }
    }
}