using Assessment.Infrastructure.Models;

namespace Assessment.Core.Interface
{
    public interface IOmdbInterface
    {
        public Task<Movie> GetMovieByTitleAsync(string title);
        public Task<ExtendedMovie> GetExtendedDetailsAsync(Movie movie);
    }
}
