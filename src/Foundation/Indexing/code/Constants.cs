namespace WageWorks.Foundation.Indexing
{
    public struct Constants
    {
        public struct IndexFields
        {
            public const string HasPresentation = "has_presentation";
            public const string AllTemplates = "all_templates";
            public const string IsLatestVersion = "_latestversion";
            public const string HasSearchResultFormatter = "has_search_result_formatter";
            public const string ContentType = "content_type";
            public const string SearchResultFormatter = "search_result_formatter";
            public const string IsAvailableOnSite = "computed_availableonsite_b";
        }

        public struct JsonFields
        {
            public const string ProductJsonField = "ProductEntityJson";
            public const string SkuJsonField = "SkuEntityJson";
            public struct ProductFields
            {
                public const string ClassificationName = "properties.ClassificationName";
                public const string ProductGroupName = "properties.ProductGroupName";
                public const string PathwayToProductGroup = "related_paths.PathwayToProductGroup[*][*].values.en-US";
                public const string DisplayOnWebsite = "properties.ProductGroupDisplayWebsite";
            }

            public struct SkuFields
            {
                public const string ClassificationName = "properties.ClassificationName";
                public const string SkuNumber = "properties.SkuNumber";
                public const string DisplayOnWebsite = "properties.SkuDisplayWebsite";
                public const string SkuName = "properties.SkuName";
                public const string SkuMarketReleaseDate = "properties.SkuMarketReleaseDate";
            }
        }
    }
}