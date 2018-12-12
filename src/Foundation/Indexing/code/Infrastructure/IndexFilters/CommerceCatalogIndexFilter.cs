using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Pipelines.IndexingFilters;
using Sitecore.Data.Items;
using System;
using System.Linq;

namespace WageWorks.Foundation.Indexing.Infrastructure.IndexFilters
{
    /// <summary>
    /// Filter out all items where workflow state does not match AllowedWorkflowStateIDs from web index
    /// </summary>
    public class CommerceCatalogWorkflowIndexFilterProcessor: InboundIndexFilterProcessor
    {
        private const string _workflowFieldName = "__Workflow state";

        /// <summary>
        /// Workflow State IDs that are allowed to go into target index
        /// </summary>
        public string AllowedWorkflowStateIDs { get; set; }

        /// <summary>
        /// Only apply when template name matches TemplateNamesToCheck in config
        /// </summary>
        public string TemplateNamesToCheck { get; set; }

        /// <summary>
        /// Only apply index on specified DBs
        /// </summary>
        public string DatabasesToFilter { get; set; }

        /// <summary>
        /// When indexed item's template name matches value from TemplateNamesToGetWorkflowStateFromParent then get WF state from parent items
        /// </summary>
        public string TemplateNamesToGetWorkflowStateFromParent { get; set; }

        public override void Process(InboundIndexFilterArgs args)
        {
            var indexable = args.IndexableToIndex as SitecoreIndexableItem;
            
            if (indexable == null)
            {
                return;
            }

            Item item = (Item)indexable;
            if (item == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(DatabasesToFilter))
            {
                var databaseNames = DatabasesToFilter.Split('|');
                if (!databaseNames.Contains(item.Database?.Name, StringComparer.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            if (!string.IsNullOrEmpty(TemplateNamesToCheck))
            {
                var templateNames = TemplateNamesToCheck.Split('|');
                if (!templateNames.Contains(item.TemplateName, StringComparer.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            if (string.IsNullOrEmpty(AllowedWorkflowStateIDs))
            {
                return;
            }
            var allowedWorkflowStateIDsList = AllowedWorkflowStateIDs.Split('|');

            if (!string.IsNullOrEmpty(TemplateNamesToGetWorkflowStateFromParent))
            {
                var templateNamesToGetWorkflowStateFromParentList = TemplateNamesToGetWorkflowStateFromParent.Split('|');
                if (templateNamesToGetWorkflowStateFromParentList.Contains(item.TemplateName, StringComparer.OrdinalIgnoreCase))
                {
                    if (item.Parent == null)
                    {
                        args.IsExcluded = true;
                        return;
                    }
                    item = item.Database?.GetItem(item.Parent.ID, item.Language, Sitecore.Data.Version.Parse(item.Version.Number));
                }
            }

            if (item == null)
            {
                args.IsExcluded = true;
                return;
            }

            //if current item's WF state doesn't match the allowed state
            var workflowStateId = item[_workflowFieldName] as string;
            if (string.IsNullOrEmpty(workflowStateId)
                || !allowedWorkflowStateIDsList.Contains(workflowStateId, StringComparer.OrdinalIgnoreCase))
            {
                args.IsExcluded = true;
                return;
            }

            //Check if ther are older versions in same WF state
            if (item.Versions.GetLaterVersions()
                .Any(i => !string.IsNullOrEmpty(i[_workflowFieldName])
                && allowedWorkflowStateIDsList.Contains(i[_workflowFieldName], StringComparer.OrdinalIgnoreCase)))
            {
                args.IsExcluded = true;
                return;
            }

        }
    }
}