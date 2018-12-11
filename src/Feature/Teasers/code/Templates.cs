namespace Wageworks.Feature.Teasers
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct DynamicTeaser
        {
            public static ID ID = new ID("{20A56D46-F5E3-4DB8-8B96-081575363D44}");

            public struct Fields
            {
                public static readonly ID Active = new ID("{9E942565-677F-491C-A0AC-6B930E37342A}");
                public static readonly ID Count = new ID("{A33F9523-96C4-4E42-B6D7-1E861718D373}");
            }
        }

        public struct TeaserHeadline
        {
            public static ID ID = new ID("{C80D124B-B9AC-432E-8C26-DBF3A7F18D20}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{4A59D072-5B41-4A79-A157-2B2CCAC10F2B}");
                public static readonly ID Icon = new ID("{3AF50903-63A9-464B-A375-B94983624E7D}");
            }
        }

        public struct TeaserContent
        {
            public static ID ID = new ID("{FEC0E62A-01FD-40E5-88F3-E5229FE79527}");

            public struct Fields
            {
                public static readonly ID Label = new ID("{3F7E7E3A-4E8E-4639-B079-FC5AEFF172F5}");
                public static readonly ID Summary = new ID("{13D97A52-7C4E-407C-960D-FADDE8A3C1B1}");
                public static readonly ID Image = new ID("{0F6B5546-E0AB-4487-81DE-640C1AA1B65B}");
                public static readonly ID Link = new ID("{E8AB122C-6F54-4D4E-AEC6-F81ADDC558FC}");
            }
        }

        public struct TeaserVideoContent
        {
            public static ID ID = new ID("{04075EB6-6D94-4BF2-9AEB-D29A89CDBA00}");

            public struct Fields
            {
                public static readonly ID VideoLink = new ID("{AC846A16-FD3F-4243-A21F-668A21010C44}");
            }
        }

        public struct Icon
        {
            public static ID ID = new ID("{E90D00B6-0BE9-48E0-9C3F-047274024270}");

            public struct Fields
            {
                public static readonly ID CssClass = new ID("{585F89D1-570C-4F66-A6EC-195A8DA654E1}");
            }
        }

        public struct RelatedProductPromo
        {
            public static ID ID = new ID("{FC16E05E-D5D3-411B-89B6-391D9639F816}");
            public struct Fields
            {
                public static readonly ID RelatedProducts = new ID("{201B6A41-3E33-4284-A06D-B46ACB4AA64F}");
            }
        }

        public struct ConditionalPromo
        {
            public static ID ID = new ID("{A543133D-0FEF-4FCA-9E97-0E5A5065E58C}");

            public struct Fields
            {
                public static readonly ID SourceField = new ID("{4E5891BE-CE79-4B64-A5E2-1DC6E2DFC883}");
                public static readonly ID ExpectedValue = new ID("{610F2FD0-43A1-454E-9A6D-E0FF49A7C967}");
            }
        }

        public struct Slide
        {
            public static ID ID = new ID("{2E1E4065-1D20-46C2-BB35-B9B3AA90D82F}");
            public struct Fields
            {
                public static readonly ID SlideStyle = new ID("{F8B765EC-3456-449E-B4AD-72BD9FD75AAD}");
            }
        }

        public struct VideoPopupCTA
        {
            public static ID ID = new ID("{17E378EB-56B5-42CF-933F-6DDF7280CD00}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{63DDA48B-B0CB-45A7-9A1B-B26DDB41009B}");
                public static readonly ID Description = new ID("{302C9F8D-F703-4F76-A4AB-73D222648232}");
                public static readonly ID Thumbnail = new ID("{4FF62B0A-D73B-4436-BEA2-023154F2FFC4}");
            }
        }

        public struct Constants
        {
            public struct JsonFields
            {
                public const string ProductJsonField = "ProductEntityJson";
                public const string SkuJsonField = "SkuEntityJson";
                public static readonly ID JsonPathField = new ID("{D8F2B090-D41C-498D-AA7E-336168B158D4}");
            }
        }

        public struct ProductVariant
        {
            public static readonly ID ID = new ID("{C92E6CD7-7F14-46E7-BBF5-29CE31262EF4}");
        }
    }
}