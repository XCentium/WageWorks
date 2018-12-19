using Sitecore;
using Sitecore.Data;
using System.Web;
using WageWorks.Foundation.Theming;
using Constants = WageWorks.Foundation.Theming.Constants;

namespace WageWorks.Feature.Teasers.Models
{
    public class ThemeModel
    {
        public HtmlString Class { get; set; }

        public static ThemeModel CreateModel(ID themeId)
        {
            var theme = Context.Database.GetItem(themeId);
            var classField = theme.Fields[Constants.ComponentParameters.Class];

            if (classField != null && !string.IsNullOrEmpty(classField.Value))
            {
                return new ThemeModel
                {
                    Class = new HtmlString(classField.Value)
                };
            }

            return new ThemeModel();
        }


        public static string GetClass(ID themeId)
        {
            var theme = Context.Database.GetItem(themeId);
            if (theme != null)
            {
                var classField = theme.Fields[Constants.ComponentParameters.Class];
                if (classField != null && !string.IsNullOrEmpty(classField.Value))
                {
                    return classField.Value;
                }
            }

            return null;
        }
    }
}