using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using System.Linq;
using WageWorks.Foundation.Theming;

namespace WageWorks.Feature.Teasers.Models
{
    public class BaseComponentModel
    {
        public string UniqueId { get; private set; }

        public string HtmlId { get; private set; } = string.Empty;

        public string SectionColorClasses { get; set; } = string.Empty;
        public string SectionSpaceClasses { get; set; } = string.Empty;

        public string SectionClasses
        {
            get
            {
                if (string.IsNullOrEmpty(SectionColorClasses))
                {
                    return SectionSpaceClasses;
                }
                else
                {
                    return SectionSpaceClasses + (!string.IsNullOrEmpty(SectionSpaceClasses) ? " " : string.Empty) + SectionColorClasses;
                }
            }
        }

        public void PopulateBaseComponent(Database database, Rendering rendering)
        {
            UniqueId = rendering.UniqueId.ToString();

            var hashtagText = rendering.Parameters[Constants.ComponentParameters.HashtagText];
            if (!string.IsNullOrEmpty(hashtagText))
            {
                HtmlId = "id=" + rendering.UniqueId.ToString();
            }

            var item = rendering.Item;
            if (item != null)
            {
                SectionColorClasses = GetThemeClasses(database, rendering, Constants.ComponentParameters.SectionColor);
                SectionSpaceClasses = GetThemeClasses(database, rendering, Constants.ComponentParameters.SpaceStyling);
            }
        }
        public static BaseComponentModel CreateComponent(Database database, Rendering rendering)
        {
            var component = new BaseComponentModel();
            component.PopulateBaseComponent(database, rendering);

            return component;
        }

        public static string GetThemeClasses(Database database, Rendering rendering, string parameterName)
        {
            var themeIds = GetParameters(rendering, parameterName);
            var themes = themeIds.Select(x => database.GetItem(new ID(x)));

            var classes = themes.Select(x => x.Fields[Constants.ComponentParameters.Class].Value);
            var results = string.Join(" ", classes);

            return results;
        }

        public static string[] GetParameters(Rendering rendering, string fieldName)
        {
            var stringParameters = rendering.Parameters[fieldName];

            if (stringParameters != null)
                return stringParameters.Split('|');
            else
                return new string[0];
        }

        public static string GetParameter(Rendering rendering, string fieldName)
        {
            var parameters = GetParameters(rendering, fieldName);
            if (parameters.Length > 0)
            {
                return parameters[0];
            }

            return string.Empty;
        }
    }
}