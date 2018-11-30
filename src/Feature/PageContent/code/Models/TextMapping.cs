using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wageworks.Feature.PageContent.Models
{
    public class TextMapping
    {
        /// <summary>
        /// Gets or sets the Regex Pattern
        /// </summary>
        public string Pattern { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets the Value to replace the <see cref="Pattern"/>
        /// </summary>
        public string Value { get; set; } = String.Empty;
    }
}