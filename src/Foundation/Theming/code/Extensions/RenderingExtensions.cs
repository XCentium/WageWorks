using System.Web.Mvc;
using Sitecore;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using WageWorks.Foundation.SitecoreExtensions.Extensions;
using WageWorks.Foundation.Theming.Extensions.Controls;

namespace WageWorks.Foundation.Theming.Extensions
{
    public static class RenderingExtensions
    {
        public static CarouselOptions GetCarouselOptions([NotNull] this Rendering rendering)
        {
            return new CarouselOptions
            {
                ItemsShown = rendering.GetIntegerParameter(Constants.CarouselLayoutParameters.ItemsShown, 3),
                AutoPlay = rendering.GetIntegerParameter(Constants.CarouselLayoutParameters.Autoplay, 1) == 1,
                ShowNavigation = rendering.GetIntegerParameter(Constants.CarouselLayoutParameters.ShowNavigation) == 1
            };
        }

        public static string GetBackgroundClass([NotNull] this Rendering rendering)
        {
            var id = MainUtil.GetID(rendering.Parameters[Constants.BackgroundLayoutParameters.Background] ?? "", null);
            if (ID.IsNullOrEmpty(id))
                return "";
            var item = rendering.RenderingItem.Database.GetItem(id);
            return item?[Templates.Style.Fields.Class] ?? "";
        }

        public static string GetCollapsibleTitle([NotNull] this Rendering rendering)
        {
            return rendering.Parameters[Constants.IsCollapsibleLayoutParameters.CollapsibleSectionTitle];
        }

        public static bool IsSectionCollapsible([NotNull] this Rendering rendering)
        {
            return MainUtil.GetBool(rendering.Parameters[Constants.IsCollapsibleLayoutParameters.IsCollapsible], false);
        }

        public static bool IsSectionCollapsedByDefault([NotNull] this Rendering rendering)
        {
            return MainUtil.GetBool(rendering.Parameters[Constants.IsCollapsibleLayoutParameters.CollapsedByDefault], false);
        }


        public static bool IsSectionDismissable([NotNull] this Rendering rendering)
        {
            return MainUtil.GetBool(rendering.Parameters[Constants.IsDismissableLayoutParameters.IsDismissable], false);
        }

        public static string GetDismissableCookieName([NotNull] this Rendering rendering)
        {
            return rendering.Parameters[Constants.IsDismissableLayoutParameters.CookieName];
        }

        public static int GetDismissableCookiePeriod([NotNull] this Rendering rendering)
        {
            return MainUtil.GetInt(rendering.Parameters[Constants.IsDismissableLayoutParameters.DismissablePeriod], 0);
        }


        public static bool IsFixedHeight([NotNull] this Rendering rendering)
        {
            var isFixed = MainUtil.GetBool(rendering.Parameters[Constants.IsFixedHeightLayoutParameters.FixedHeight] ?? "", false);
            return isFixed;
        }

        public static int GetHeight([NotNull] this Rendering rendering)
        {
            return MainUtil.GetInt(rendering.Parameters[Constants.IsFixedHeightLayoutParameters.Height] ?? "", 0);
        }

        public static string GetContainerClass([NotNull] this Rendering rendering)
        {
            return rendering.IsContainerFluid() ? "container-fluid" : "container";
        }

        public static bool IsContainerFluid([NotNull] this Rendering rendering)
        {
            return MainUtil.GetBool(rendering.Parameters[Constants.HasContainerLayoutParameters.IsFluid], false);
        }

        public static BackgroundRendering RenderBackground([NotNull] this Rendering rendering, HtmlHelper helper)
        {
            return new BackgroundRendering(helper.ViewContext.Writer, rendering.GetBackgroundClass());
        }
    }
}