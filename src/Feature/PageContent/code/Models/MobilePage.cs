using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace WageWorks.Feature.PageContent.Models
{
    [SitecoreType(AutoMap = true)]
    public class MobilePage
    {
        #region Properties
        [SitecoreIgnore]
        public string View { get; set; } = "generic_content";

        public virtual string NavigationTitle { get; set; }

        public virtual string Title { get; set; }

        public virtual string Summary { get; set; }

        public virtual string Body { get; set; }
 

 
        #endregion
    }
}