using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc.Configuration.Attributes;

namespace WageWorks.Foundation.Indexing.Models.Glass
{
    [SitecoreType(TemplateId = "{5C125B6D-C481-4C24-B5B9-9A23FE396BF0}")]
    public partial interface IFacet
    {
        #region Facet

        [SitecoreField(FieldId = "{31CA077F-EF38-4980-8A24-05408031B2BC}")]
        string Name { get; set; }

        [SitecoreField(FieldId = "{5DE17054-0141-4E58-935B-FEC623A60806}")]
        string DisplayName { get; set; }

        [SitecoreField(FieldId = "{FAE92C1E-9E2C-4209-89EB-C256D55E3D7F}")]
        string Type { get; set; }

        [SitecoreField(FieldId = "{0E23BB51-C071-41B5-9D83-AC410B89B85A}")]
        string FieldName { get; set; }


        #endregion
    }
}