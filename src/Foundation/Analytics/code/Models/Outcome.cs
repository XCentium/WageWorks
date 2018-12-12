using System;
using System.Collections.Generic;

namespace WageWorks.Foundation.Analytics.Models
{
    public class Outcome : ISupportCustomValues
    {
        public Outcome()
        {
            this.CustomValues = new Dictionary<string, string>();
        }

        public decimal MonetaryValue { get; set; }
        public Guid DefinitionId { get; set; }
        public Dictionary<string, string> CustomValues { get; set; }
        public DateTime DateTime { get; set; }
    }
}