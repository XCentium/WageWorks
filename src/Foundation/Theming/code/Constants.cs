namespace WageWorks.Foundation.Theming
{
    public struct Constants
    {
        public struct HasContainerLayoutParameters
        {
            public static string IsFluid => "ContainerIsFluid";
        }

        public struct BackgroundLayoutParameters
        {
            public static string Background => "Background";
        }

        public struct IsFixedHeightLayoutParameters
        {
            public static string FixedHeight => "Fixed height";

            public static string Height => "Height";
        }

        public struct CarouselLayoutParameters
        {
            public static string ItemsShown => "ItemsShown";
            public static string Autoplay => "Autoplay";
            public static string ShowNavigation => "ShowNavigation";
        }

        public struct PromoLayoutParameters
        {
            public static string CssVariant => "CSS Variants";
            public static string CssFieldName => "CSS Class Name";
        }

        public struct IsCollapsibleLayoutParameters
        {
            public static string IsCollapsible => "Is Collapsible";
            public static string CollapsedByDefault => "Collapsed By Default";

            public static string CollapsibleSectionTitle => "Collapsible Section Title";
        }

        public struct IsDismissableLayoutParameters
        {
            public static string IsDismissable => "Is Dismissable";

            public static string DismissablePeriod => "Dismissable Period";
            public static string CookieName => "Cookie Name";
        }

        public struct GuideSelector
        {
            public static string SeelAllText => "See All Text";

            public static string SeeAllLink => "See All Link";
        }

        public struct Datasources
        {
            public static string Link  => "{F0714F7D-22D3-476B-8690-B61D01840CA3}";

            public static string IconLink  => "{B4211822-18DF-4003-AD2B-2FF7CB4DD371}";

            public static string Guide  => "{E5EE0784-976D-42CF-B4C1-473329B4E131}";
            public static string HowTo => "{3828B1CF-C64F-4F25-A6C6-E8F5A6544E82}";

            public static string Tool => "{85F2A4F0-A270-40FB-BD5C-A2EF99530F63}";
            public static string Article  => "{F91BDE69-D07F-45B0-8224-09111DFCC214}";
        }
        public struct ComponentParameters
        {
            public static string SpaceStyling => "Space Styling";

            public static string SectionColor => "Section Color";

           public static string Layouts => "Layout";
            public static string HashtagText  => "Hashtag Text";
            public static string Class => "class";
            public static string Name  => "Name";
            public static string Text => "Text";
            public static string Url => "Url";
            public static string IconTheme => "Icon Theme";

        }


        public struct Fields
        {
        public static string Image => "Image";
            public static string BackgroundImage => "Background Image";

            public static string CardColor => "Card Color";

            public static string Name => "Name";
            public static string Title => "Title";
            public static string SectionTitle => "Section Title";


            public static string Description => "Description";
            public static string Headline => "Headline";

            public static string Items => "Items";
            public static string HashtagText  => "Hashtag Text";
            public static string Shadow  => "Shadow";

            public static string OpenItemText  => "Open Item Text";

        }
    }
}