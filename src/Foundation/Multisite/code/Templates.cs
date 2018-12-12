using Sitecore.Data;

namespace WageWorks.Foundation.Multisite
{
    public class Templates
    {
        public struct Site
        {
            public static ID ID = new ID("{BB85C5C2-9F87-48CE-8012-AF67CF4F765D}");
        }

        public struct DatasourceConfiguration
        {
            public static ID ID = new ID("{C82DC5FF-09EF-4403-96D3-3CAF377B8C5B}");

            public struct Fields
            {
                public static readonly ID DatasourceLocation = new ID("{5FE1CC43-F86C-459C-A379-CD75950D85AF}");
                public static readonly ID DatasourceTemplate = new ID("{498DD5B6-7DAE-44A7-9213-1D32596AD14F}");
            }
        }

        public struct SiteSettings
        {
            public static ID ID = new ID("{BCCFEBEA-DCCB-48FE-9570-6503829EC03F}");
        }

        public struct SiteSettingsRoot
        {
            public static ID ID = new ID("{4C82B6DD-FE7C-4144-BCB3-F21B4080568F}");
        }

        public struct RenderingOptions
        {
            public static ID ID = new ID("{D1592226-3898-4CE2-B190-090FD5F84A4C}");

            public struct Fields
            {
                public static readonly ID DatasourceLocation = new ID("{B5B27AF1-25EF-405C-87CE-369B3A004016}");
                public static readonly ID DatasourceTemplate = new ID("{1A7C85E5-DC0B-490D-9187-BB1DBCB4C72F}");
            }
        }

        #region -- Site Configuration --

        public struct ConfigurationSettingFolder
        {
            public static readonly ID ID = new ID("{103E5566-416F-4668-B441-421A1C5A1270}");
        }

        public struct ConfigurationSetting
        {
            public static readonly ID ID = new ID("{3E6CA8E9-934D-4459-A8C9-1DC650A42686}");

            public struct Fields
            {
                public static readonly ID Name = new ID("{214AF02B-5E69-406E-A1B8-4F6C252E7935}");
                public static readonly ID Category = new ID("{573B1019-2EA8-4839-A667-4A0437182DCC}");
                public static readonly ID Value = new ID("{26AF9F32-CF51-48C8-B584-1B5A05195678}");
                public static readonly ID Description = new ID("{72FCA0C7-767D-4033-967B-34C76D64B575}");
            }
        }

        #endregion

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
                public static readonly ID SkuJson = new ID("{A6572C81-71A7-4FD8-87A8-0EF583281228}");
            }
        }

        public struct Product
        {
            public static readonly ID ID = new ID("{225F8638-2611-4841-9B89-19A5440A1DA1}");

            public struct Fields
            {

            }
        }

        public struct DAMItem
        {
            public static readonly ID ID = new ID("{646B1739-85DE-49AF-AC12-32C5704B34DF}");

            public struct Fields
            {
                public static readonly ID EntityId = new ID("{97BEEB98-2FD8-431E-B877-7C7BA24EE84A}");
            }
        }




    }
}