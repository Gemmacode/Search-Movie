using Assessment.Core.Interface;
using Assessment.Infrastructure.Models;
using Newtonsoft.Json;

namespace Assessment.Core.Service
{
    public class OmdbService : IOmdbInterface
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "f51e7b79";

        public OmdbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Movie> GetMovieByTitleAsync(string title)
        {
            var response = await _httpClient.GetAsync($"http://www.omdbapi.com/?apikey={_apiKey}&t={title}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Movie>(content);
            }
            //Handle errors appropriately
            return null;
        }



        public async Task<ExtendedMovie> GetExtendedDetailsAsync(Movie movie)
        {
            if (movie == null || string.IsNullOrEmpty(movie.Title))
            {
                return null;
            }

            var basicDetailsResponse = await _httpClient.GetAsync($"http://www.omdbapi.com/?apikey={_apiKey}&t={Uri.EscapeDataString(movie.Title)}");

            if (basicDetailsResponse.IsSuccessStatusCode)
            {
                var basicDetailsContent = await basicDetailsResponse.Content.ReadAsStringAsync();
                var basicDetails = JsonConvert.DeserializeObject<ExtendedMovie>(basicDetailsContent);

                if (!string.IsNullOrEmpty(basicDetails?.ImdbId))
                {
                    var extendedDetailsResponse = await _httpClient.GetAsync($"http://www.omdbapi.com/?apikey={_apiKey}&i={basicDetails.ImdbId}");
                    if (extendedDetailsResponse.IsSuccessStatusCode)
                    {
                        var extendedDetailsContent = await extendedDetailsResponse.Content.ReadAsStringAsync();
                        var extendedDetails = JsonConvert.DeserializeObject<ExtendedMovie>(extendedDetailsContent);
                        if (extendedDetails != null)
                        {
                            basicDetails.Description = extendedDetails.Description;
                            basicDetails.ImdbScore = extendedDetails.ImdbScore;
                        }
                    }
                }

                return basicDetails;
            }

            return null;
        }

    }
}
