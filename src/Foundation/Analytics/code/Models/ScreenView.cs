using System;
using System.Collections.Generic;

namespace Wageworks.Foundation.Analytics.Models
{
    public class ScreenView: ISupportCustomValues
    {
        public ScreenView()
        {
            this.Events = new List<Event>();
            this.CustomValues = new Dictionary<string, string>();
        }

        public Guid ContentItemId { get; set; }
        public List<Event> Events { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string ScreenshotData { get; set; }
        public Dictionary<string, string> CustomValues { get; set; }
    }
}