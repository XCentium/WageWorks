using Newtonsoft.Json.Linq;
using Sitecore.Caching;
using Sitecore.Caching.Generics;
using Sitecore.Collections;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using WageWorks.Foundation.Indexing.Services;

namespace WageWorks.Foundation.Indexing.Infrastructure.Fields
{
    public class JsonPathComputedFieldBase
    {
        protected internal ICache<string> IndexingCache;
        private const string _indexingCacheName = "indexing_cache";
        private const long _cacheSize = 512000000;
        public IList<string> AllowedTemplates { get; set; }
        public string RootItemPath { get; set; }
        public Dictionary<string, string> JsonPaths { get; set; }
        public string JsonPath { get; set; }
        public string JsonPathsFolder { get; set; }
        public string JsonFieldName { get; set; }
        public string FieldName { get; set; }
        public string ReturnType { get; set; }
        public int ArrayPosition { get; set; }
        public Guid SortFolder { get; set; }


        public JsonPathComputedFieldBase(XmlNode configNode)
        {
            IndexingCache = CacheManager.FindCacheByName<string>(_indexingCacheName);
            this.JsonFieldName = XmlUtil.GetAttribute("jsonFieldName", configNode);
            if (string.IsNullOrEmpty(JsonFieldName))
            {
                throw new ArgumentNullException("JsonFieldName", "JsonFieldName parameter is not configured on JsonPathValueComputedField. Check index configuration.");
            }

            this.JsonPathsFolder = XmlUtil.GetAttribute("jsonPathsFolder", configNode);
            this.JsonPath = XmlUtil.GetAttribute("jsonPath", configNode);
            var jsonPaths = XmlUtil.GetChildElements("jsonPath", configNode);
            if (jsonPaths != null && jsonPaths.Count() > 0)
            {
                JsonPaths = new Dictionary<string, string>();
                foreach (XmlNode path in jsonPaths)
                {
                    var displayname = XmlUtil.GetAttribute("displayName", path);
                    JsonPaths.Add(path.InnerText,
                        !string.IsNullOrWhiteSpace(displayname) ? displayname : string.Empty);
                }
                //JsonPaths = jsonPaths.Select(e => e.InnerText).ToList();
            }
            if (string.IsNullOrEmpty(JsonPath) && (jsonPaths == null || jsonPaths.Count() == 0) && (string.IsNullOrEmpty(JsonPathsFolder)))
            {
                throw new ArgumentNullException("JsonPath", "At least one jsonPath attribute OR element(s) is required. Check index configuration.");
            }
            if (IndexingCache == null)
            {
                IndexingCache = new Cache<string>(_indexingCacheName, _cacheSize);
            }
            this.RootItemPath = XmlUtil.GetAttribute("rootItemPath", configNode);

            var allowedTemplatesString = XmlUtil.GetAttribute("allowedTemplates", configNode);
            if (!string.IsNullOrEmpty(allowedTemplatesString))
            {
                this.AllowedTemplates = allowedTemplatesString.Split(',').ToList();
            }

            this.ArrayPosition = XmlUtil.HasAttribute("arrayPosition", configNode)
                ? Int32.Parse(XmlUtil.GetAttribute("arrayPosition", configNode))
                : -1;

            this.SortFolder = XmlUtil.HasAttribute("sortFolder", configNode)
                ? Guid.Parse(XmlUtil.GetAttribute("sortFolder", configNode))
                : Guid.Empty;


        }

        public string GetConfigAttribute(XmlNode configNode, string fieldName, bool throwErrorIfNotFound = false)
        {
            var configValue = XmlUtil.GetAttribute(fieldName, configNode);
            if (throwErrorIfNotFound && string.IsNullOrEmpty(configValue))
            {
                throw new ArgumentNullException(fieldName, string.Format("'{0}' parameter is not configured on JsonPathValueComputedField. Check index configuration.", fieldName));
            }

            return configValue;
        }

        public Item GetItem(IIndexable indexable)
        {
            Item item = (Item)(indexable as SitecoreIndexableItem);
            if (item == null)
            {
                return null;
            }

            if (AllowedTemplates != null && !AllowedTemplates.Contains(item.TemplateName, StringComparer.OrdinalIgnoreCase))
            {
                return null;
            }

            if (!string.IsNullOrEmpty(RootItemPath) && !item.Paths.FullPath.StartsWith(RootItemPath, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return item;
        }

        public string GetTokenValue(Item item, string fieldName, string jsonPath)
        {
            var result = GetTokenValues(item, fieldName, jsonPath);
            return result?.FirstOrDefault();
        }

        public ChildList GetSortItems()
        {
            if (this.SortFolder == Guid.Empty)
            {
                return null;
            }

            ChildList sortValues = null;
            var cacheKey = string.Format("{0}-{1}-SortValues", this.FieldName, this.SortFolder);
            if (IndexingCache.ContainsKey(cacheKey))
            {
                sortValues = IndexingCache.GetValue(cacheKey) as ChildList;
            }
            else
            {
                var db = Sitecore.Configuration.Factory.GetDatabase("master");
                var parentItem = db?.GetItem(new ID(this.SortFolder));
                if (parentItem == null) return null;

                sortValues = parentItem.Children;
                IndexingCache.Add(cacheKey, parentItem.Children, new TimeSpan(2, 0, 0));
            }

            return sortValues;
        }

        protected IEnumerable<string> GetTokenValues(Item item, string fieldName, string jsonPath)
        {
            var itemField = item.Fields[fieldName];

            if (itemField == null)
            {
                return null;
            }
            JObject json;
            var cacheKey = string.Format("{0}-{1}", item.ID.Guid, fieldName);
            if (IndexingCache.ContainsKey(cacheKey))
            {
                json = IndexingCache.GetValue(cacheKey) as JObject;
            }
            else
            {
                var fieldValue = itemField.Value;
                if (string.IsNullOrEmpty(fieldValue))
                {
                    return null;
                }
                json = JObject.Parse(fieldValue);
                IndexingCache.Add(cacheKey, json, new TimeSpan(0, 2, 0));
            }

            if (json != null)
            {
                var tokens = json.SelectTokens(jsonPath)
                .Where(e => e != null && !string.IsNullOrEmpty(e.Value<string>()))
                .Select(x => x.Value<string>());

                return tokens != null && tokens.Count() > 0 ? tokens.ToList<string>() : null;
            }

            return null;
        }
        protected IEnumerable<object> TryParse(IEnumerable<string> values, bool forceType = false)
        {
            if (values == null)
            {
                return null;
            }


            return (from value in values.Select(v => TryParse(v, forceType))
                    where value != null
                    select value);
        }

        protected object TryParse(string value, bool forceType = false)
        {
            //Try to cast value to known types or return as is
            if (!string.IsNullOrEmpty(ReturnType))
            {
                var returnType = this.ReturnType.ToLower();

                if (returnType.Contains("datetime"))
                {
                    DateTime convertedDate;
                    if (DateTime.TryParse(value, out convertedDate))
                        return convertedDate;
                    else if (forceType)
                        return null;
                }
                if (returnType.Contains("bool"))
                {
                    bool convertedBool;
                    if (bool.TryParse(value, out convertedBool))
                        return convertedBool;
                    else if (forceType)
                        return null;
                }
                if (returnType.Contains("int"))
                {
                    int convertedInt;
                    if (int.TryParse(value, out convertedInt))
                        return convertedInt;
                    else if (forceType)
                        return null;
                }
                if (returnType.Contains("float"))
                {
                    float convertedFloat;
                    if (float.TryParse(value, out convertedFloat))
                        return convertedFloat;
                    else if (forceType)
                        return null;
                }
            }

            return value;
        }

        protected IEnumerable<string> GetChildItemsTokenValues(Item item, string fieldName, string jsonPath)
        {
            var resultList = new List<string>();
            foreach (Item childItem in item.GetChildren())
            {
                if (childItem != null && IndexExtensions.IsAvailableOnSite(childItem))
                {
                    var results = GetTokenValues(childItem, fieldName, jsonPath);
                    if (results != null)
                    {
                        foreach (var result in results)
                        {
                            if (result != null && !resultList.Contains(result))
                            {
                                resultList.Add(result);
                            }
                        }
                    }
                }
            }

            return resultList.Count > 0 ? resultList : null;
        }

        protected IEnumerable<DateTime> GetChildItemsTokenDateTimeValues(Item item, string fieldName, string jsonPath)
        {
            var resultList = new List<string>();
            var resultDates = new List<DateTime>();
            foreach (Item childItem in item.GetChildren())
            {
                if (childItem != null)
                {
                    var results = GetTokenValues(childItem, fieldName, jsonPath);
                    if (results != null)
                    {
                        foreach (var result in results)
                        {
                            if (result != null && !resultList.Contains(result.ToString()))
                            {
                                DateTime convertedResult;
                                if (DateTime.TryParse(result, out convertedResult))
                                {
                                    resultList.Add(result);
                                    resultDates.Add(convertedResult);
                                }
                            }
                        }
                    }
                }
            }

            return resultDates.Count > 0 ? resultDates : null;
        }
    }
}