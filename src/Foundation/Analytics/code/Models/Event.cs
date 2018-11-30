using System;
using System.Collections.Generic;

namespace Wageworks.Foundation.Analytics.Models
{
    public class Event : ISupportCustomValues
    {
        public Event()
        {
            this.CustomValues = new Dictionary<string, string>();
        }
        public Guid DefinitionId { get; set; }
        public string DataKey { get; set; }
        public string Data { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, string> CustomValues { get; set; }
    }
}
