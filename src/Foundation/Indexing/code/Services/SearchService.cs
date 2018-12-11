using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Extensions;
using Wageworks.Foundation.Indexing.Models;
using Wageworks.Foundation.Indexing.Repositories;
using Wageworks.Foundation.Solr.SpatialSearch;

namespace Wageworks.Foundation.Indexing.Services
{
    public class SearchService
    {
        public SearchService(ISearchSettings settings)
        {
            this.Settings = settings;
            this.SearchIndexResolver = DependencyResolver.Current.GetService<SearchIndexResolver>();
            this.SearchResultsFactory = DependencyResolver.Current.GetService<SearchResultsFactory>();
        }

        public virtual ISearchSettings Settings { get; set; }

        public SitecoreIndexableItem ContextItem
        {
            get
            {
                var contextItem = this.Settings.Root ?? Context.Item;
                Assert.IsNotNull(contextItem, "Could not determine a context item for the search");
                return contextItem;
            }
        }

        private SearchIndexResolver SearchIndexResolver { get; }

        private SearchResultsFactory SearchResultsFactory { get; }

        public IEnumerable<IQueryRoot> QueryRoots => IndexingProviderRepository.QueryRootProviders.Union(new[] {this.Settings });

    
        public virtual ISearchResults Search(IQuery query)
        {
            //using (var context = this.SearchIndexResolver.GetIndex(this.ContextItem).CreateSearchContext())
            var index = ContentSearchManager.GetIndex(GetProductIndex());
            using (var context = index.CreateSearchContext())
            {
                var queryable = this.CreateAndInitializeQuery(context, query);

                queryable = this.AddContentPredicates(queryable, query);
                queryable = this.AddFacets(queryable, query);
                queryable = this.AddPaging(queryable, query);
                queryable = this.SortByScore(queryable);
                //if (query.InitialFilters != null && query.InitialFilters.Any())
                //    queryable = this.AddFilters(queryable, query);

                var results = queryable.GetResults();
                return this.SearchResultsFactory.Create(results, query);
            }
        }

       
        public static string GetProductIndex()
        {
            var site = Sitecore.Context.Site;
            var sitename = site?.Name.ToLowerInvariant() ?? "website";

            var db = Sitecore.Context.Database ?? Sitecore.Context.ContentDatabase;
            var dbName = db.Name.ToLowerInvariant();

            var indexProperty = site?.Properties["productSearchIndex"];

            var productIndex = (string.IsNullOrWhiteSpace(indexProperty)) ? $"{sitename}_{dbName}_index" : indexProperty;

            return productIndex;
        }

        public virtual ISearchResults SpatialSearch(ISpatialQuery query, ISearchIndex index = null)
        {
            if(index == null)
            {
                index = this.SearchIndexResolver.GetIndex(this.ContextItem);
            }

            using (var context = index.CreateSearchContext())
            {
                var queryable = this.CreateAndInitializeSpatialQuery(context);

                queryable = this.AddSpatialRadiusPredicate(queryable, query);
                queryable = queryable.Page(0, query.NoOfResults);
                var results = queryable.GetResults();
                return this.SearchResultsFactory.CreateSpatial(results, query);
            }
        }

        private IQueryable<SearchResultItem> SortByScore(IQueryable<SearchResultItem> queryable)
        {
            return queryable.OrderByDescending(i => i["score"]);
        }


        private IQueryable<SearchResultItem> AddPaging(IQueryable<SearchResultItem> queryable, IQuery query)
        {
            return queryable.Page(query.Page < 0 ? 0 : query.Page, query.NoOfResults <= 0 ? 10 : query.NoOfResults);
        }

        public IQueryable<SearchResultItem> AddFilters(IQueryable<SearchResultItem> queryable, IQuery query)
        {
            var isStaging = Wageworks.Foundation.SitecoreExtensions.Extensions.SiteExtensions.GetBoolSiteSetting("staging");

            foreach (var filter in query.InitialFilters)
            {
                var ignoreOnStaging = filter[Templates.SearchFacetConfigurations.Fields.IgnoreOnStaging].ToBool();
                if (isStaging && ignoreOnStaging) continue;

                var facetField = filter[Templates.SearchFacetConfigurations.Fields.Facet];
                if (string.IsNullOrWhiteSpace(facetField)) continue;

                var facetFieldName = Sitecore.Context.Database.GetItem(facetField)?[Templates.Facet.Fields.FieldName];
                if (string.IsNullOrWhiteSpace(facetFieldName)) continue;

                var value = filter[Templates.SearchFacetConfigurations.Fields.FilterValue];
                if (string.IsNullOrWhiteSpace(value)) continue;

                var stagingValue = filter[Templates.SearchFacetConfigurations.Fields.StagingFilterValue];
                if (isStaging && !string.IsNullOrWhiteSpace(stagingValue))
                {
                    var stagingValues = stagingValue.Split('|', false);
                    queryable = queryable.Filter(item => stagingValues.Contains(item[facetFieldName]));
                }
                else
                {
                    var values = value.Split('|', false);
                    queryable = queryable.Filter(item => values.Contains(item[facetFieldName]));
                }
            }
            return queryable;
        }

        public Expression<Func<SearchResultItem, bool>> AddFilters(IQuery query)
        {
            var predicate = PredicateBuilder.True<SearchResultItem>();
        
           var isStaging = Wageworks.Foundation.SitecoreExtensions.Extensions.SiteExtensions.GetBoolSiteSetting("staging");

            foreach (var filter in query.InitialFilters)
            {
                var ignoreOnStaging = filter[Templates.SearchFacetConfigurations.Fields.IgnoreOnStaging].ToBool();
                if (isStaging && ignoreOnStaging) continue;

                var facetField = filter[Templates.SearchFacetConfigurations.Fields.Facet];
                if (string.IsNullOrWhiteSpace(facetField)) continue;

                var facetFieldName = Sitecore.Context.Database.GetItem(facetField)?[Templates.Facet.Fields.FieldName];
                if (string.IsNullOrWhiteSpace(facetFieldName)) continue;

                var value = filter[Templates.SearchFacetConfigurations.Fields.FilterValue];
                if (string.IsNullOrWhiteSpace(value)) continue;

                var stagingValue = filter[Templates.SearchFacetConfigurations.Fields.StagingFilterValue];
                if (isStaging && !string.IsNullOrWhiteSpace(stagingValue))
                {
                    var stagingValues = stagingValue.Split('|', false);
                    predicate = predicate.And(item => stagingValues.Contains(item[facetFieldName]));
                }
                else
                {
                    var values = value.Split('|', false);
                    predicate = predicate.And(item => values.Contains(item[facetFieldName]));
                }
            }
            return predicate;
        }

        public virtual ISearchResults FindAll()
        {
            return this.FindAll(0, 0);
        }

        public virtual ISearchResults FindAll(int skip, int take)
        {
            using (var context = ContentSearchManager.GetIndex(this.ContextItem).CreateSearchContext())
            {
                var queryable = this.CreateAndInitializeQuery(context);

                if (skip > 0)
                {
                    queryable = queryable.Skip(skip);
                }
                if (take > 0)
                {
                    queryable = queryable.Take(take);
                }

                var results = queryable.GetResults();
                return this.SearchResultsFactory.Create(results, null, false);
            }
        }

        public virtual FacetResults FindAllFacets(IEnumerable<IQueryFacet> facets)
        {
            using (var context = ContentSearchManager.GetIndex(this.ContextItem).CreateSearchContext())
            {
                var queryable = this.CreateAndInitializeQuery(context);
                queryable = AddFacets(queryable, facets);
                
                var results = queryable.GetResults();
                return results.Facets;
                //return this.SearchResultsFactory.Create(results, null, false);
            }
        }

        private IQueryable<SearchResultItem> CreateAndInitializeQuery(IProviderSearchContext context, IQuery query = null)
        {
            var queryable = context.GetQueryable<SearchResultItem>();
            queryable = this.InitializeQuery(queryable, query);
            return queryable;
        }

        private IQueryable<SpatialSearchResultItem> CreateAndInitializeSpatialQuery(IProviderSearchContext context)
        {
            var queryable = context.GetQueryable<SpatialSearchResultItem>();
            var query = this.InitializeQuery(queryable);
            return query.Cast<SpatialSearchResultItem>();
        }

        private IQueryable<SearchResultItem> InitializeQuery(IQueryable<SearchResultItem> queryable, IQuery query = null)
        {
            queryable = this.SetQueryRoots(queryable, query);
            queryable = this.FilterOnLanguage(queryable);
            queryable = this.FilterOnVersion(queryable);
            if (this.Settings.MustHaveFormatter)
            {
                queryable = this.FilterOnHasSearchResultFormatter(queryable);
            }
            if (this.Settings.Templates != null && this.Settings.Templates.Any())
            {
                queryable = queryable.Cast<IndexedItem>().Where(this.GetTemplatePredicates(this.Settings.Templates));
            }
            else
            {
                queryable = this.FilterOnItemsMarkedAsIndexable(queryable, query);
            }
            return queryable;
        }

        private IQueryable<SearchResultItem> FilterOnHasSearchResultFormatter(IQueryable queryable)
        {
            return queryable.Cast<IndexedItem>().Where(i => i.HasSearchResultFormatter);
        }

        private IQueryable<SearchResultItem> FilterOnItemsMarkedAsIndexable(IQueryable<SearchResultItem> queryable, IQuery query = null)
        {
            var indexedItemPredicate = this.GetPredicateForItemDerivesFromIndexedItem();
            var contentTemplatePredicates = this.GetPredicatesForContentTemplates(query);
            return queryable.Cast<IndexedItem>().Where(indexedItemPredicate.And(contentTemplatePredicates));
        }

        private Expression<Func<IndexedItem, bool>> GetPredicatesForContentTemplates(IQuery query = null)
        {
            var contentTemplatePredicates = PredicateBuilder.False<IndexedItem>();
            foreach (var provider in IndexingProviderRepository.QueryPredicateProviders)
            {
                contentTemplatePredicates = contentTemplatePredicates.Or(this.GetTemplatePredicates(provider.SupportedTemplates));
            }

            if (query != null)
            {
                foreach (var provider in IndexingProviderRepository.QueryPredicateProviders)
                {
                    contentTemplatePredicates = contentTemplatePredicates
                        .Or(this.GetTemplatePredicates(provider.SupportedTemplatesWithQuery(query)));
                }
            }
            
            return contentTemplatePredicates;
        }

        private Expression<Func<IndexedItem, bool>> GetPredicateForItemDerivesFromIndexedItem()
        {
            var notIndexedItem = PredicateBuilder.Create<IndexedItem>(i => !i.AllTemplates.Contains(IdHelper.NormalizeGuid(Templates.IndexedItem.ID)));
            var indexedItemWithShowInResults = PredicateBuilder.And<IndexedItem>(i => i.AllTemplates.Contains(IdHelper.NormalizeGuid(Templates.IndexedItem.ID)), i => i.ShowInSearchResults);

            return notIndexedItem.Or(indexedItemWithShowInResults);
        }

        private Expression<Func<IndexedItem, bool>> GetTemplatePredicates(IEnumerable<ID> templates)
        {
            var expression = PredicateBuilder.False<IndexedItem>();
            foreach (var template in templates)
            {
                expression = expression.Or(i => i.AllTemplates.Contains(IdHelper.NormalizeGuid(template)));
            }
            return expression;
        }

        private IQueryable<SearchResultItem> AddFacets(IQueryable<SearchResultItem> queryable, IQuery query)
        {
            var facets = GetFacetsFromProviders();

            var addedFacetPredicate = false;
            var facetPredicate = PredicateBuilder.True<SearchResultItem>();
            foreach (var facet in facets)
            {
                if (string.IsNullOrEmpty(facet.FieldName))
                    continue;

                if (query.Facets != null && query.Facets.ContainsKey(facet.FieldName))
                {
                    var facetValues = query.Facets[facet.FieldName];

                    var facetValuePredicate = PredicateBuilder.False<SearchResultItem>();
                    foreach (var facetValue in facetValues)
                    {
                        if (facetValue == null)
                            continue;
                        facetValuePredicate = facetValuePredicate.Or(item => item[facet.FieldName] == facetValue);
                    }
                    facetPredicate = facetPredicate.And(facetValuePredicate);
                    addedFacetPredicate = true;
                }
                queryable = queryable.FacetOn(item => item[facet.FieldName]);
            }
            if (addedFacetPredicate)
                queryable = queryable.Where(facetPredicate);

            return queryable;
        }

        private IQueryable<SearchResultItem> AddFacets(IQueryable<SearchResultItem> queryable, IEnumerable<IQueryFacet> facets)
        {
            var facetPredicate = PredicateBuilder.True<SearchResultItem>();
            foreach (var facet in facets)
            {
                if (string.IsNullOrEmpty(facet?.FieldName))
                    continue;

                queryable = queryable.FacetOn(item => item[facet.FieldName]);

            }

            return queryable;
        }

        private static IEnumerable<IQueryFacet> GetFacetsFromProviders()
        {
            return IndexingProviderRepository.QueryFacetProviders.SelectMany(provider => provider.GetFacets()).Distinct(new GenericEqualityComparer<IQueryFacet>((facet, queryFacet) => facet.FieldName == queryFacet.FieldName, facet => facet.FieldName.GetHashCode()));
        }

        private IQueryable<SearchResultItem> FilterOnLanguage(IQueryable<SearchResultItem> queryable)
        {
            queryable = queryable.Filter(item => item.Language == Context.Language.Name);
            return queryable;
        }

        private IQueryable<SearchResultItem> FilterOnVersion(IQueryable<SearchResultItem> queryable)
        {
            queryable = queryable.Cast<IndexedItem>().Filter(item => item.IsLatestVersion);
            return queryable;
        }

        private IQueryable<SearchResultItem> SetQueryRoots(IQueryable<SearchResultItem> queryable, IQuery query = null)
        {
            var rootPredicates = PredicateBuilder.False<SearchResultItem>();

            foreach (var provider in this.QueryRoots)
            {
                if (provider.Root == null)
                {
                    continue;
                }
                rootPredicates = rootPredicates.Or(item => item.Path.StartsWith(provider.Root.Paths.FullPath));
            }

            //TODO: Move this into provider
            //add product path for now.
            if (query != null && query.IncludeProducts)
            {
                var productRoot = GetRootItem();
                if (productRoot != null)
                    rootPredicates = rootPredicates.Or(item => item.Path.StartsWith(productRoot.Paths.FullPath));
            }

            return queryable.Where(rootPredicates);
        }
        private Item GetRootItem()
        {
            var site = Sitecore.Context.Site;
            var productsRoot = site?.Properties["productsCatalogRoot"];
            return site?.Database?.GetItem(productsRoot);
        }
        private IQueryable<SearchResultItem> AddContentPredicates(IQueryable<SearchResultItem> queryable, IQuery query)
        {
            var contentPredicates = PredicateBuilder.False<SearchResultItem>();
            foreach (var provider in IndexingProviderRepository.QueryPredicateProviders)
            {
                var fieldPredicate = provider.GetQueryPredicate(query);

                if (provider.ProviderName == "products")
                {
                    if (query.InitialFilters != null && query.InitialFilters.Any())
                    {
                        fieldPredicate = fieldPredicate.And(this.AddFilters(query));
                    }
                }
               
                contentPredicates = contentPredicates.Or(fieldPredicate);
            }
            return queryable.Where(contentPredicates);
        }

        private IQueryable<SpatialSearchResultItem> AddSpatialRadiusPredicate(IQueryable<SpatialSearchResultItem> queryable, ISpatialQuery query)
        {
            queryable = queryable.WithinRadius(i => i.Location, query.Origin.Lat, query.Origin.Lon, query.Radius);
            return queryable.OrderByNearest();
        }

      
    }
}