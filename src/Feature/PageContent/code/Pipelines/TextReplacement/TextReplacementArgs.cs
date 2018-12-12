using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Pipelines;

namespace WageWorks.Feature.PageContent.Pipelines.TextReplacement
{
    public class TextReplacementArgs : PipelineArgs
    {
        /// <summary>
        /// Gets or sets the content containing shortcodes
        /// </summary>
        public string Content { get; set; }
    }
}