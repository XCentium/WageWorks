using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Diagnostics;
using Sitecore.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace WageWorks.Foundation.Indexing.Infrastructure.Fields
{
    /// <summary>
    /// Set _latestversion value to true on items that match criteria cpecified in congfig 
    /// </summary>
    public class CommerceLatestVersionComputedField : IComputedIndexField
    {
        private const string _workflowFieldName = "__Workflow state";
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        /// <summary>
        /// Only apply when indexed item's template name matches config
        /// </summary>
        public string AllowedTemplates { get; set; }

        /// <summary>
        /// Workflow ID(s) to match (exclude the rest)
        /// </summary>
        public string AllowedWorkflowIDs { get; set; }

        /// <summary>
        /// When indexed item's template name matches value from TemplateNamesToGetWorkflowStateFromParent then get WF state from parent items
        /// </summary>
        public string TemplateNamesToGetWorkflowStateFromParent { get; set; }

        private List<string> AllowedTemplatesList { get; set; }
        private List<string> AllowedWorkflowIDsList { get; set; }
        private List<string> TemplateNamesToGetWorkflowStateFromParentList { get; set; }

        public CommerceLatestVersionComputedField(XmlNode configNode) : base()
        {
            this.AllowedTemplates = XmlUtil.GetAttribute("allowedTemplates", configNode);
            if (string.IsNullOrEmpty(AllowedTemplates))
            {
                Log.Error($"CommerceLatestVersionComputedField: AllowedTemplates is a required parameter. Check computed field configuration", this);
            }
            this.AllowedTemplatesList = AllowedTemplates.Split('|').ToList();

            this.AllowedWorkflowIDs = XmlUtil.GetAttribute("allowedWorkflowIDs", configNode);
            if (string.IsNullOrEmpty(AllowedWorkflowIDs))
            {
                Log.Error($"CommerceLatestVersionComputedField: AllowedWorkflowIDs is a required parameter. Check computed field configuration", this);
            }
            this.AllowedWorkflowIDsList = AllowedWorkflowIDs.Split('|').ToList();

            this.TemplateNamesToGetWorkflowStateFromParent = XmlUtil.GetAttribute("templateNamesToGetWorkflowStateFromParent", configNode);
            if (string.IsNullOrEmpty(TemplateNamesToGetWorkflowStateFromParent))
            {
                Log.Error($"CommerceLatestVersionComputedField: TemplateNamesToGetWorkflowStateFromParent is a required parameter. Check computed field configuration", this);
            }
            this.TemplateNamesToGetWorkflowStateFromParentList = TemplateNamesToGetWorkflowStateFromParent.Split('|').ToList();
        }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexItem = indexable as SitecoreIndexableItem;
            if (indexItem == null)
            {
                return null;
            }

            var item = indexItem.Item;
            if (item == null)
            {
                return null;
            }

            if (!AllowedTemplatesList.Contains(item.TemplateName, StringComparer.OrdinalIgnoreCase))
            {
                return null;
            }

            if (TemplateNamesToGetWorkflowStateFromParentList.Contains(item.TemplateName, StringComparer.OrdinalIgnoreCase))
            {
                if (item.Parent == null)
                {
                    return null;
                }
                item = item.Database?.GetItem(item.Parent.ID, item.Language, Sitecore.Data.Version.Parse(item.Version.Number));
            }

            var workflowStateId = item[_workflowFieldName] as string;
            var databaseName = item.Database?.Name?.ToLower();

            if (databaseName.Equals("web", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(workflowStateId)
                        && AllowedWorkflowIDsList.Contains(workflowStateId, StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                } 
            }
            else if (databaseName.Equals("master", StringComparison.OrdinalIgnoreCase))
            {
                var latestVersion = item.Versions.GetLatestVersion(item.Language);
                if (latestVersion != null && latestVersion.Version != null && item.Version != null && latestVersion.Version.Number == item.Version.Number)
                {
                    return true;
                }
            }

            return null;

        }
    }
}