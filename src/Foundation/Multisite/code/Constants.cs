namespace WageWorks.Foundation.Multisite
{
    public struct Constants
    {
        public struct JsonFields
        {
            public const string ProductJsonField = "ProductEntityJson";
            public const string SkuJsonField = "SkuEntityJson";
            public struct ProductFields
            {
                public const string ClassificationName = "properties.ClassificationName";
                public const string ProductGroupName = "properties.ProductGroupName";
                public const string PathwayToProductGroup = "related_paths.PathwayToProductGroup[*][*].values['en-US']";
            }

            public struct SkuFields
            {
                public const string ClassificationName = "properties.ClassificationName";
                public const string SkuNumber = "properties.SkuNumber";
            }
        }
    }
}