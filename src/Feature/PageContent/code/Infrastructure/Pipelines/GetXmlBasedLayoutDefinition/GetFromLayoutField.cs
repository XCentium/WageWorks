using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Pipelines.Response.GetXmlBasedLayoutDefinition;
using Sitecore.Mvc.Presentation;
using Sitecore.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;

namespace WageWorks.Feature.PageContent.Infrastructure.Pipelines.GetXmlBasedLayoutDefinition
{
    public class GetFromLayoutField : GetXmlBasedLayoutDefinitionProcessor
    {
        public override void Process(GetXmlBasedLayoutDefinitionArgs args)
        {
            if (args.Result != null || PageContext.Current.Item == null) return;

            string key = string.Format("LayoutXml.{0}.{1}",
                Context.Language.Name,
                PageContext.Current.Item.ID);

            XElement pageLayoutXml = (XElement)HttpContext.Current.Cache[key];
            if (pageLayoutXml != null && Context.PageMode.IsNormal)
            {
                args.Result = pageLayoutXml;
                return;
            }

            XElement content = GetFromField(PageContext.Current.Item);
            if (content != null && (Context.PageMode.IsPreview || Context.PageMode.IsNormal))
            {
                Item item = PageContext.Current.Item;
                if (item != null &&
                    !item.TemplateID.Equals(Templates.HeaderPage.ID) &&
                    !item.TemplateID.Equals(Templates.FooterPage.ID))
                {
                    XElement currentPageDxElement = content.Element("d");
                    if (currentPageDxElement == null)
                    {
                        args.Result = content;
                        return;
                    }

                    SiteContext siteInfo = SiteContext.Current;
                    if (siteInfo != null)
                    {
                        if (!string.IsNullOrEmpty(siteInfo.Properties["headerItem"]))
                        {
                            currentPageDxElement.Add(GetContent(siteInfo.Properties["headerItem"], "zone-Header"));
                        }

                        if (!string.IsNullOrEmpty(siteInfo.Properties["footerItem"]))
                        {
                            currentPageDxElement.Add(GetContent(siteInfo.Properties["footerItem"], "zone-Footer"));
                        }
                    }
                }
            }

            try
            {
                HttpContext.Current.Cache.Add(
                    key,
                    content,
                    null,
                    DateTime.Now.AddMinutes(5),
                    Cache.NoSlidingExpiration,
                    CacheItemPriority.AboveNormal,
                    null);
            }
            catch (Exception ex)
            {
                var i = ex; // TODO: Log exception?
            }

            args.Result = content;
        }
        /// <summary>
        /// From given Item's Layout extract the elements with given placeholder.
        /// </summary>
        /// <param name="fromItemId"></param>
        /// <param name="fromItemPlaceholder"></param>
        /// <returns></returns>
        private List<XElement> GetContent(string fromItemId, string fromItemPlaceholder)
        {
            if (string.IsNullOrEmpty(fromItemId) || string.IsNullOrEmpty(fromItemPlaceholder))
            {
                return new List<XElement>();
            }

            XElement inputLayout = GetItemLayout(fromItemId);
            if (inputLayout != null)
            {
                XElement dXElementInInputLayout = inputLayout.Element("d");
                if (dXElementInInputLayout != null)
                {
                    return dXElementInInputLayout.Elements().ToList();
                }
            }

            return new List<XElement>();
        }

        /// <summary>
        /// Returns Layout Field Value for given ItemID
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        private XElement GetItemLayout(string itemId)
        {
            Item currentItem = Context.Database.GetItem(itemId);
            if (currentItem != null)
            {
                return GetFromField(currentItem);
            }
            return null;
        }


        /// <summary>
        /// Gets the xml from the current items layout field
        /// </summary>
        /// <returns></returns>
        private XElement GetFromField(Item item)
        {
            if (item == null)
            {
                return null;
            }

            Field field = item.Fields[FieldIDs.FinalLayoutField];
            if (field == null)
            {
                return null;
            }

            string fieldValue = LayoutField.GetFieldValue(field);
            if (fieldValue.IsWhiteSpaceOrNull())
            {
                return null;
            }

            return XDocument.Parse(fieldValue).Root;
        }

        /// <summary>
        /// Extracts the elements that have given placeholder.
        /// </summary>
        /// <param name="layoutElements"></param>
        /// <param name="placeholder"></param>
        /// <returns></returns>
        private List<XElement> ExtractContentsFromLayout(IEnumerable<XElement> layoutElements, string placeholder)
        {
            Log.Debug(string.Format("GetFromLayoutField - ExtractContentsFromLayout : Starting to extract for placeholder {0}:", placeholder));

            List<XElement> elements = new List<XElement>();

            var lEelements = layoutElements.ToList();
            if (lEelements.Any())
            {
                foreach (XElement element in lEelements)
                {
                    if (element != null && element.HasAttributes)
                    {
                        if (element.Attribute("ph") != null)
                        {
                            var xAttribute = element.Attribute("ph");
                            if (xAttribute != null)
                            {
                                string value = xAttribute.Value;
                                if (!string.IsNullOrEmpty(value) && (value.StartsWith("/" + placeholder) || value.Equals(placeholder)))
                                {
                                    elements.Add(element);
                                }
                            }
                        }
                    }
                }
            }
            Log.Debug("GetFromLayoutField - ExtractContentsFromLayout : Done with extract");

            return elements;
        }
    }
}