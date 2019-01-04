using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WageWorks.Feature.Teasers.Models
{
    public interface IItem
    {
        string DisplayName { get; }
        ID TemplateId { get; }
        Item Item { get; }

        Database Database { get; }
        
    }
}
