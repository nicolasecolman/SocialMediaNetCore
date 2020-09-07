using SocialMedia.Core.QueryFilters;
using System;

namespace SocialMedia.Infrastructure.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl)
        {
            var url = $"{_baseUri}{actionUrl}";
            return new Uri(url);
        }

    }
}
