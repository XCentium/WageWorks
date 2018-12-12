using Sitecore.Data;

namespace WageWorks.Foundation.Indexing
{
    internal struct Templates
    {
        internal struct IndexedItem
        {
            public static ID ID = new ID("{8FD6C8B6-A9A4-4322-947E-90CE3D94916D}");

            public struct Fields
            {
                public static readonly ID IncludeInSearchResults = new ID("{8D5C486E-A0E3-4DBE-9A4A-CDFF93594BDA}");
                public const string IncludeInSearchResults_FieldName = "IncludeInSearchResults";
            }
        }
        public struct ProductAttribute
        {
            public static readonly ID ID = new ID("{F727B77A-8667-4FF7-BA5C-1524A96D6A97}");

            public struct Fields
            {
                public static readonly ID ProductAttributeJsonPath = new ID("{D8F2B090-D41C-498D-AA7E-336168B158D4}");
                public static readonly ID ProductAttributeName = new ID("{DC5D9B23-11EC-4E72-9F3C-76CE1B4C869B}");
            }
        }
        public struct ProductVariant
        {
            public static readonly ID ID = new ID("{C92E6CD7-7F14-46E7-BBF5-29CE31262EF4}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{BDCCCBF4-F075-48B5-B9BF-F8810829DB69}");
                public const string Title_FieldName = "DisplayName";

                //public static readonly ID Date = new ID("{C464D2D7-3382-428A-BCDF-0963C60BA0E3}");

                public static readonly ID Summary = new ID("{F0408018-3D25-4729-83AF-AD2A83644252}");
                public const string Summary_FieldName = "ProductGroupOverview";

                //public static readonly ID Body = new ID("{801612C7-5E98-4E3C-80D2-A34D0EEBCBDA}");
                //public const string Body_FieldName = "NewsBody";
                public static readonly ID ProductJson = new ID("{F52CDF32-8AAD-47CD-8EEB-D59DBF0E53C4}");
                public static readonly string SkuEntityJson = "SkuEntityJson";

                public static readonly string IsDeleted = "IsSkuDeleted";
            }
        }

        public struct SearchFacetConfigurations
        {
            public struct Fields
            {
                public static readonly ID Facet = new ID("{3A94CA14-3F35-4768-BFB6-5532F10A6A3D}");
                public static readonly ID FilterValue = new ID("{DBDDDF29-DD21-4151-92C4-927C1C30AFFC}");
                public static readonly ID StagingFilterValue = new ID("{C1D84BD7-8D6A-48A6-8229-C0E55C37B021}");
                public static readonly ID IgnoreOnStaging = new ID("{4FCBF4C8-B175-401E-A40C-B495F1D9A169}");
            }
        }

        public struct Facet
        {
            public static readonly ID ID = new ID("{5C125B6D-C481-4C24-B5B9-9A23FE396BF0}");
            public struct Fields
            {
                public static readonly ID FieldName = new ID("{0E23BB51-C071-41B5-9D83-AC410B89B85A}");
            }
        }

        public struct Product
        {
            public static readonly ID ID = new ID("{225F8638-2611-4841-9B89-19A5440A1DA1}");

            public struct Fields
            {
                public static readonly string IsDeleted = "IsProductDeleted";
            }
        }
    }
}