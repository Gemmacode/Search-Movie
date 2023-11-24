using Newtonsoft.Json;

namespace Assessment.Infrastructure.Models
{
    public class SearchResult
    {
        [JsonProperty("Search")]
        public List<Movie> Search { get; set; }
    }
}
