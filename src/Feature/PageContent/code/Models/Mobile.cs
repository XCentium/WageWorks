using Glass.Mapper.Sc.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Configuration;

namespace WageWorks.Feature.PageContent.Models
{
    [SitecoreType(AutoMap = true)]
    public class Mobile 
    {
        #region Properties
        [SitecoreId]
        public virtual Guid Id { get; set; }
        [SitecoreInfo(SitecoreInfoType.DisplayName)]
        public virtual string Name { get; set; }
        [SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; set; }
        [SitecoreInfo(SitecoreInfoType.Path)]
        public virtual string Path { get; set; }

        public virtual IEnumerable<MobilePage> Children { get; set; }


        #endregion
    }
}