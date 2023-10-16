using eagles_food_backend.Domains.Filters;

namespace eagles_food_backend.Services.UriService
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route, string? searchTerm = null);
    }
}
