﻿using eagles_food_backend.Domains.Filters;

using Microsoft.AspNetCore.WebUtilities;

namespace eagles_food_backend.Services.UriService
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var _endpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = string.IsNullOrEmpty(filter.SearchTerm)
                ? _endpointUri.ToString()
                : QueryHelpers.AddQueryString(_endpointUri.ToString(), "searchTerm", filter.SearchTerm);
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());

            return new Uri(modifiedUri);
        }
    }
}
