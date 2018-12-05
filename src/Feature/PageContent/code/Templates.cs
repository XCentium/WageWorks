namespace Vista.Feature.PageContent
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct HeaderPage
        {
            public static readonly ID ID = new ID("{B0D45EE4-8E04-4076-8EDF-EEF62612E24E}");
        }
        public struct FooterPage
        {
            public static readonly ID ID = new ID("{173BCB10-1A41-47D3-A317-8C20AC4ED756}");
        }

        public struct HasPageContent
        {
            public static readonly ID ID = new ID("{AF74A00B-8CA7-4C9A-A5C1-156A68590EE2}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{C30A013F-3CC8-4961-9837-1C483277084A}");
                public const string Title_FieldName = "title_t";
                public static readonly ID Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
                public const string Summary_FieldName = "summary_t";
                public static readonly ID Body = new ID("{D74F396D-5C5E-4916-BD0A-BFD58B6B1967}");
                public const string Body_FieldName = "body_t";

                public static readonly ID FooterBody = new ID("{50126513-4167-43A2-8A94-BD642A6B8A2F}");
                public const string FooterBody_FieldName = "footer_body_t";

                public static readonly ID BodyFooter = new ID("{50126513-4167-43A2-8A94-BD642A6B8A2F}");
                public static readonly ID Image = new ID("{9492E0BB-9DF9-46E7-8188-EC795C4ADE44}");
            }
        }

        public struct RichTextContent
        {
            public static readonly ID ID = new ID("{46DA80D0-0E78-41FD-A4FA-20FF654ACC3F}");

            public struct Fields
            {
                public static readonly ID Body = new ID("{7259C20B-8826-4D55-92F3-7F120FCA1005}");
            }
        }

        public struct HeadinePage
        {
            public static readonly ID ID = new ID("{11E8A65D-326E-4D1D-8FE9-109E918C0A1D}");

            public struct Fields
            {
                public static readonly ID Headline = new ID("{A053BBAB-844F-4E3D-A570-491119D53AE2}");
                public static readonly ID Subheadline = new ID("{6FABF1EE-9BC1-42FA-B4B6-E3DC30A1421B}");
                public static readonly ID SubheadlineBackground = new ID("{98794A67-CB8A-4AE9-982C-9E5CFF27BC8A}");
                public static readonly ID SubheadlineForeground = new ID("{2CE3B315-9116-490C-AEF6-4407A78C43B0}");
            }
        }
    }
}