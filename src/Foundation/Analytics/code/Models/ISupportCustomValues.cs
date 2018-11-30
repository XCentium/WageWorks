using System.Collections.Generic;

namespace Wageworks.Foundation.Analytics.Models
{
    public interface ISupportCustomValues
    {
        Dictionary<string, string> CustomValues { get; set; }
    }
}