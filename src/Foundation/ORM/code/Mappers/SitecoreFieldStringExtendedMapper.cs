using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.DataMappers;
using Sitecore.Data.Fields;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WageWorks.Foundation.ORM.Mappers
{
    public class SitecoreFieldStringExtendedMapper : SitecoreFieldStringMapper
    {
        /// <summary>
        /// Field Types that can contain text that should run through the RenderField pipeline
        /// </summary>
        private static readonly IList<string> AllowedFieldTypes = new[] { "rich text", "multi-line text", "single-line text" };

        /// <summary>
        /// Gets the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="config">The config.</param>
        /// <param name="context">The context.</param>
        /// <returns>System.Object.</returns>
        public override object GetField(Field field, SitecoreFieldConfiguration config, SitecoreDataMappingContext context)
        {
            if (field == null)
            {
                return string.Empty;
            }

            if (field.Name.StartsWith("__")) return string.Empty;

            if (config.Setting == SitecoreFieldSettings.RichTextRaw)
            {
                return field.Value;
            }

            if (config.Setting == SitecoreFieldSettings.ForceRenderField)
            {
                return this.RunPipeline(field);
            }

            var runRenderPipeline = AllowedFieldTypes.Any(type => field.TypeKey.Equals(type, StringComparison.InvariantCultureIgnoreCase));

            return this.GetResult(field, runRenderPipeline);
        }

        /// <summary>
        /// Gets the Result of the Field
        /// </summary>
        /// <param name="field">The Field</param>
        /// <param name="runRenderPipeline"><c>True</c> if the field should run through the renderField pipeline.</param>
        /// <returns>The field value</returns>
        protected override string GetResult(Field field, bool runRenderPipeline)
        {
            string result = null;

            if (field.DisplayName == "Updated by")
                return string.Empty;

            try
            {
                result = !runRenderPipeline ? field.Value : this.RunPipeline(field);
            }
            catch (Exception ex)
            {
                // TODO: Log exception
            }

            return result;
        }
    }
}