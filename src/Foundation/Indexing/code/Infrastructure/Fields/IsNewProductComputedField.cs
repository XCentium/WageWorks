using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Wageworks.Foundation.Dictionary.Repositories;
using Wageworks.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Diagnostics;

namespace Wageworks.Foundation.Indexing.Infrastructure.Fields
{
    public class IsNewProductComputedField : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            var item = indexable as SitecoreIndexableItem;
            if (item?.Item == null) return null;

            try
            {
                var convertedDate = new DateTime();
                var parsed = false;

                if (item.Item.Template.ID == Templates.Product.ID)
                {
                    convertedDate = GetMinDate(item.Item, Constants.JsonFields.SkuJsonField,
                        Constants.JsonFields.SkuFields.SkuMarketReleaseDate);
                    if (convertedDate != DateTime.MinValue)
                        parsed = true;
                }
                else if (item.Item.Template.ID == Templates.ProductVariant.ID)
                {
                    var marketReleaseDate = JsonExtensions.GetTokenValues(item, Constants.JsonFields.SkuJsonField,
                        Constants.JsonFields.SkuFields.SkuMarketReleaseDate)?.FirstOrDefault();

                    if (DateTime.TryParse(marketReleaseDate, out convertedDate))
                        parsed = true;
                }


                return parsed && DateTime.UtcNow < convertedDate.AddYears(1);
            }
            catch (Exception ex)
            {
                Log.Warn($"Could not retrieve attribute for compute field: Field Name: {this.FieldName}, Item Id: {item.Item.ID}, Item Name: {item.Item.Name},", ex, this);
                return null;
            }
        }

        private DateTime GetMinDate(Item item, string fieldName, string jsonPath)
        {
            var dateResults = GetChildItemsTokenDateTimeValues(item, fieldName, jsonPath);
            if (dateResults == null || !dateResults.Any())
            {
                return DateTime.MinValue;
            }

            return dateResults.Min();
        }

        protected IEnumerable<DateTime> GetChildItemsTokenDateTimeValues(Item item, string fieldName, string jsonPath)
        {
            var resultList = new List<string>();
            var resultDates = new List<DateTime>();
            foreach (Item childItem in item.GetChildren())
            {
                if (childItem == null) continue;

                var results = JsonExtensions.GetTokenValues(childItem, fieldName, jsonPath);
                if (results == null) continue;

                foreach (var result in results)
                {
                    if (result == null || resultList.Contains(result.ToString())) continue;
                    DateTime convertedResult;
                    if (!DateTime.TryParse(result, out convertedResult)) continue;
                    resultList.Add(result);
                    resultDates.Add(convertedResult);
                }
            }

            return resultDates.Count > 0 ? resultDates : null;
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}