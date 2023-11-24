using Assessment.Core.Interface;
using Assessment.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Assessment.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class OmdbController : ControllerBase
    {
        private readonly IOmdbInterface _omdbService;
        private readonly IMemoryCache _memoryCache;

        public OmdbController(IOmdbInterface omdbService, IMemoryCache memoryCache)
        {
            _omdbService = omdbService;
            _memoryCache = memoryCache;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Movie>>> SearchMovies(string query)
        {            
            var movie = await _omdbService.GetMovieByTitleAsync(query);

            if (movie != null)
            {
                var searchQuery = new SearchQuery { Query = query, SearchTime = DateTime.Now };
                var recentSearches = _memoryCache.Get<List<SearchQuery>>("RecentSearches") ?? new List<SearchQuery>();
                recentSearches.Insert(0, searchQuery);
                recentSearches = recentSearches.Take(5).ToList();
                _memoryCache.Set("RecentSearches", recentSearches);
                return Ok(new List<Movie> { movie });
            }
            return NotFound();
        }

      
        [HttpGet("extended-info")]
        public async Task<ActionResult<ExtendedMovie>> GetExtendedInfo(string title)
        {
            var movie = await _omdbService.GetMovieByTitleAsync(title);

            if (movie != null)
            {
                var extendedMovie = await _omdbService.GetExtendedDetailsAsync(movie);

                if (extendedMovie != null)
                {
                    return Ok(extendedMovie);
                }
            }

            return NotFound();
        }

        [HttpGet("recent-searches")]
        public ActionResult<IEnumerable<SearchQuery>> GetRecentSearches()
        {
            var recentSearches = _memoryCache.Get<IEnumerable<SearchQuery>>("RecentSearches") ?? Enumerable.Empty<SearchQuery>();
            return Ok(recentSearches.OrderByDescending(s => s.SearchTime).Take(5));
        }

    }
}
