using System;
using System.Collections.Generic;

namespace Wageworks.Foundation.Analytics.Models
{
    public class Interaction: ISupportCustomValues
    {
        public Interaction()
        {
            this.ScreenViews = new List<ScreenView>();
            this.CustomValues = new Dictionary<string, string>();
            this.Outcomes = new List<Outcome>();
        }

        public Guid? CampaignId { get; set; }
        public Guid ChannelId { get; set; }
        public Guid? VenueId { get; set; }
        public Dictionary<string, string> CustomValues { get; set; }
        public List<ScreenView> ScreenViews { get; set; }
        public List<Outcome> Outcomes { get; set; }
        public string SiteName { get; set; }
        //todo: add profiles?
    }
}