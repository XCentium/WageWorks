using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WageWorks.Feature.PageContent.Models;

namespace WageWorks.Feature.PageContent.Pipelines.TextReplacement
{
    public class TextReplacementProcessor
    {
        /// <summary>
        /// Gets or sets the Text Mappings
        /// <para>Set by Sitecores Configuration Factory on instantiation</para>
        /// </summary>
        public virtual List<TextMapping> TextMappings { get; set; } = new List<TextMapping>();

        /// <summary>
        /// Gets if the Context is currently editing
        /// </summary>
        public virtual bool IsEditing { get { return Sitecore.Context.PageMode.IsExperienceEditorEditing; } }

        /// <summary>
        /// Process the <see cref="args"/> and replaces markup based on the configured <see cref="TextMappings"/>
        /// </summary>
        /// <param name="args">The args to process</param>
        public virtual void Process(TextReplacementArgs args)
        {
            if (this.IsEditing || String.IsNullOrEmpty(args.Content))
            {
                return;
            }

            if (!this.TextMappings.Any())
            {
                return;
            }

            args.Content = this.TextMappings.Aggregate(args.Content,
                (content, mapping) => Regex.Replace(content, mapping.Pattern, mapping.Value));
        }
    }
}