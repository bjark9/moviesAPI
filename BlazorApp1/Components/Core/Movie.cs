using System.Text.Encodings.Web;
using System.Text.Json.Serialization;

namespace BlazorApp1.Components.Core
{
    public class TmdbResponse
    {
        [JsonPropertyName("results")] // When program sees "results" in JSON file -> maps it to property "Results" 
        public List<Movie> Results {get;set;}
    }
    public class Movie
    {
        [JsonPropertyName("id")]
        public int ID {get;set;}

        [JsonPropertyName("title")]
        public string? Title {get;set;}

        [JsonPropertyName("tagline")]
        public string? TagLine {get;set;}

        [JsonPropertyName("popularity")]
        public float? Popularity {get;set;}

        [JsonPropertyName("overview")]
        public string? Overview {get;set;}

        [JsonPropertyName("release_date")]
        public string? ReleaseDate {get;set;}
        public UrlEncoder? Image {get;set;} // string??
    }
}