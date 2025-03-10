using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.ViewModels
{
    public class SearchViewModel
    {
        public string Query { get; set; }
        public string Type { get; set; } // "all", "tasks", "users", "comments"
        public Dictionary<string, List<string>> Filters { get; set; }
        public SearchSortOptions SortOptions { get; set; }
        public SearchPagination Pagination { get; set; }

        public SearchViewModel()
        {
            Filters = new Dictionary<string, List<string>>();
            SortOptions = new SearchSortOptions();
            Pagination = new SearchPagination();
        }
    }

    public class SearchSortOptions
    {
        public string Field { get; set; }
        public bool Ascending { get; set; }
        public string SortBy { get; set; }
    }

    public class SearchPagination
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    }

    public class SearchResultViewModel
    {
        public List<SearchResultItem> Items { get; set; }
        public int TotalResults { get; set; }
        public Dictionary<string, List<string>> Facets { get; set; }
        public List<string> Suggestions { get; set; }
        public SearchPagination Pagination { get; set; }
        public Dictionary<string, object> Metadata { get; set; }

        public SearchResultViewModel()
        {
            Items = new List<SearchResultItem>();
            Facets = new Dictionary<string, List<string>>();
            Suggestions = new List<string>();
            Metadata = new Dictionary<string, object>();
        }
    }

    public class SearchResultItem
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public List<string> Highlights { get; set; }

        public SearchResultItem()
        {
            Metadata = new Dictionary<string, object>();
            Highlights = new List<string>();
        }
    }

    public class AdvancedSearchViewModel
    {
        public string Query { get; set; }
        public List<string> Types { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
        public List<int> UserIds { get; set; }
        public bool IncludeArchived { get; set; }
        public Dictionary<string, object> CustomFilters { get; set; }

        public AdvancedSearchViewModel()
        {
            Types = new List<string>();
            Categories = new List<string>();
            Tags = new List<string>();
            UserIds = new List<int>();
            CustomFilters = new Dictionary<string, object>();
        }
    }

    public class SearchFilterViewModel
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
        public bool IsExact { get; set; }
        public bool IsCaseSensitive { get; set; }
    }

    public class SearchSuggestionViewModel
    {
        public string Text { get; set; }
        public string Type { get; set; }
        public double Score { get; set; }
        public Dictionary<string, object> Metadata { get; set; }

        public SearchSuggestionViewModel()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    public class SearchAnalyticsViewModel
    {
        public List<SearchTerm> PopularSearches { get; set; }
        public List<SearchTerm> RecentSearches { get; set; }
        public Dictionary<string, int> SearchesByType { get; set; }
        public Dictionary<string, double> AverageResultCounts { get; set; }
        public List<NoResultSearch> NoResultSearches { get; set; }

        public SearchAnalyticsViewModel()
        {
            PopularSearches = new List<SearchTerm>();
            RecentSearches = new List<SearchTerm>();
            SearchesByType = new Dictionary<string, int>();
            AverageResultCounts = new Dictionary<string, double>();
            NoResultSearches = new List<NoResultSearch>();
        }
    }

    public class SearchTerm
    {
        public string Query { get; set; }
        public int Count { get; set; }
        public DateTime LastSearched { get; set; }
        public double AverageResults { get; set; }
        public List<string> RelatedQueries { get; set; }

        public SearchTerm()
        {
            RelatedQueries = new List<string>();
        }
    }

    public class NoResultSearch
    {
        public string Query { get; set; }
        public DateTime SearchedAt { get; set; }
        public string UserAgent { get; set; }
        public List<string> SuggestedAlternatives { get; set; }

        public NoResultSearch()
        {
            SuggestedAlternatives = new List<string>();
        }
    }

    public class SearchHistoryViewModel
    {
        public int UserId { get; set; }
        public List<SearchHistoryItem> Searches { get; set; }
        public Dictionary<string, int> SearchesByCategory { get; set; }
        public List<SavedSearch> SavedSearches { get; set; }

        public SearchHistoryViewModel()
        {
            Searches = new List<SearchHistoryItem>();
            SearchesByCategory = new Dictionary<string, int>();
            SavedSearches = new List<SavedSearch>();
        }
    }

    public class SearchHistoryItem
    {
        public string Query { get; set; }
        public DateTime SearchedAt { get; set; }
        public int ResultCount { get; set; }
        public List<string> Filters { get; set; }
        public List<string> ClickedResults { get; set; }

        public SearchHistoryItem()
        {
            Filters = new List<string>();
            ClickedResults = new List<string>();
        }
    }

    public class SavedSearch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Query { get; set; }
        public Dictionary<string, object> Filters { get; set; }
        public bool NotifyOnResults { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastExecuted { get; set; }

        public SavedSearch()
        {
            Filters = new Dictionary<string, object>();
        }
    }
}
