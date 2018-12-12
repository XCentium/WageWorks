using System.Collections.Generic;

namespace WageWorks.Foundation.Analytics.Models
{
    public interface ISupportCustomValues
    {
        Dictionary<string, string> CustomValues { get; set; }
    }
}