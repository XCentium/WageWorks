using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WageWorks.Feature.Teasers.Models
{
    public class PlainCardModel :ContentInfoModel
    {
        public static PlainCardModel CreateModel(ID itemId)
        {
            var item = Sitecore.Context.Database.GetItem(itemId);
            var model = new PlainCardModel();
            model.PopulateContentInfoModel(item, null, null, null);

            return model;
        }
    }
}