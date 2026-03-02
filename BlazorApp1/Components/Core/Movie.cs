using System.Text.Encodings.Web;

namespace BlazorApp1.Components.Core
{
    public class Movie
    {
        public string? Title {get;set;}
        public string? TagLine {get;set;}
        public float? Popularity {get;set;}
        public string? Overview {get;set;}
        public string? Date {get;set;}
        public UrlEncoder? Image {get;set;} // string??
    }
}