using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Abstractions;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.RenderField;
using Wageworks.Feature.PageContent.Pipelines.TextReplacement;

namespace Wageworks.Feature.PageContent.Pipelines.RenderField
{
    public class RunTextReplacement
    {
        /// <summary>
        /// Sitecore Pipeline Manager Implementation
        /// </summary>
        private readonly BaseCorePipelineManager _pipelineRunner;

        /// <summary>
        /// Field Types that can contain text that should be replaced
        /// </summary>
        private static readonly IList<string> AllowedFieldTypes = new[] { "rich text", "multi-line text", "single-line text" };

        public RunTextReplacement(BaseCorePipelineManager pipelineRunner)
        {
            this._pipelineRunner = pipelineRunner;
        }

        /// <summary>
        /// Main method called within RenderField pipeline
        /// </summary>
        /// <param name="args"></param>
        public void Process(RenderFieldArgs args)
        {
            if (!this.CanFieldBeProcessed(args))
            {
                return;
            }

            args.Result.FirstPart = this.ReplaceText(args.Result.FirstPart);
        }

        /// <summary>
        /// Verifies that the field being rendered should be processed
        /// </summary>
        /// <param name="args">The args</param>
        /// <returns><c>True</c> if the field data can be processed</returns>
        public virtual bool CanFieldBeProcessed(RenderFieldArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.FieldTypeKey, "args.FieldTypeKey");

            var fieldTypeKey = args.FieldTypeKey.ToLower();

            return RunTextReplacement.AllowedFieldTypes.Any(f => f.Equals(fieldTypeKey));
        }

        /// <summary>
        /// Replaces text within the Content by running the textReplacement pipeline
        /// </summary>
        /// <param name="content">The content</param>
        /// <returns>Content with values replaced</returns>
        public virtual string ReplaceText(string content)
        {
            if (String.IsNullOrEmpty(content))
            {
                return content;
            }

            var args = new TextReplacementArgs
            {
                Content = content
            };

            this._pipelineRunner.Run("textReplacement", args);

            return args.Content;
        }

    }
}