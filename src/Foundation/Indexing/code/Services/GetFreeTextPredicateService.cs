using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using WageWorks.Foundation.Indexing.Models;

namespace WageWorks.Foundation.Indexing.Services
{
    public static class GetFreeTextPredicateService
    {
        public static Expression<Func<SearchResultItem, bool>> GetFreeTextPredicate(string[] fieldNames, IQuery query,
            bool boost = false, float boostValue = 1f)
        {
            var predicate = PredicateBuilder.False<SearchResultItem>();
            if (string.IsNullOrWhiteSpace(query.QueryText))
            {
                return predicate;
            }

            var querySplitted = query.QueryText.Split(' ');
            foreach (var name in fieldNames)
            {
                var wordPredicate = PredicateBuilder.True<SearchResultItem>();
                foreach (var word in querySplitted)
                {
                    wordPredicate = boost 
                        ? wordPredicate.And(item => item[name].Contains(word).Boost(boostValue)) 
                        : wordPredicate.And(item => item[name].Contains(word));
                }

                predicate = predicate.Or(wordPredicate);
                ////predicate = predicate.Or(i => i[name].Contains(query.QueryText));
                //predicate = predicate.Or(i => i[name].MatchWildcard($"*{query.QueryText}*"));
            }
            return predicate;
        }

        public static Expression<Func<T, bool>> GetFreeTextPredicate<T>(string[] fieldNames, IQuery query) where T : SearchResultItem
        {
            var predicate = PredicateBuilder.False<T>();
            if (string.IsNullOrWhiteSpace(query.QueryText))
            {
                return predicate;
            }
            var querySplitted = query.QueryText.Split(' ');
            foreach (var name in fieldNames)
            {
                var wordPredicate = PredicateBuilder.True<T>();
                foreach (var word in querySplitted)
                {
                    wordPredicate = wordPredicate.And(item => item[name].Contains(word));
                }

                //predicate = predicate.Or(i => i[name].Contains(query.QueryText));
                predicate = predicate.Or(wordPredicate);
                //predicate = predicate.Or(i => i[name].MatchWildcard($"*{query.QueryText}*"));
            }
            return predicate;
        }

        public static Expression<Func<SearchResultItem, bool>> GetFreeTextPredicate(Dictionary<string, float> fieldNames, IQuery query)
        {
            var predicate = PredicateBuilder.False<SearchResultItem>();
            if (string.IsNullOrWhiteSpace(query.QueryText))
            {
                return predicate;
            }

            var querySplitted = query.QueryText.Split(' ');
            foreach (var name in fieldNames)
            {
                var wordPredicate = PredicateBuilder.True<SearchResultItem>();
                foreach (var word in querySplitted)
                {
                    wordPredicate = wordPredicate.And(item => item[name.Key].Contains(word).Boost(name.Value));

                }

                predicate = predicate.Or(wordPredicate);
                ////predicate = predicate.Or(i => i[name].Contains(query.QueryText));
                //predicate = predicate.Or(i => i[name].MatchWildcard($"*{query.QueryText}*"));
            }
            return predicate;
        }
    }
}