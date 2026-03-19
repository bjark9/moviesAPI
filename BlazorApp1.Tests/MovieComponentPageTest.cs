using Bunit;
using Xunit;
using BlazorApp1.Components.Core;
using BlazorApp1.Components.Pages;
using RichardSzalay.MockHttp;
using System.Text.Json;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

public class MovieComponentPageTest : TestContext
{
    private readonly FakeSearchState fakeSearchState;

    public MovieComponentPageTest()
    {
        fakeSearchState = new FakeSearchState();   // create _searchState object
        Services.AddScoped<SearchState>(_ => fakeSearchState);    // Add the object to the Singleton service

        // new services required by the modified components
        var userService = new UserService();
        Services.AddSingleton<UserService>(userService);
        Services.AddSingleton<FavoritesState>(new FavoritesState(userService));
    }

    private void SetupHttpClient(List<Movie> movies)
    {
        // "Mock" a HttpClient that we will use in the tests 
        var mockHttp = new MockHttpMessageHandler();  
        var payload = JsonSerializer.Serialize(new TmdbResponse{Results= movies}); 
        mockHttp
            .When("https://api.themoviedb.org/*")
            .Respond("application/json", payload);
        Services.AddSingleton<HttpClient>(mockHttp.ToHttpClient());
    }

    [Fact]
    public void ShowLoadingBeforeDataArrives()
    {
        // Arrange – no HTTP setup so movies stays null
        fakeSearchState.SetQuery("matrix");
        // Create a HTTP request and store it as a Singleton service
        var mockHttp = new MockHttpMessageHandler(); // Create Fake HTTP handler
        // Defines what to return when a request matches, here we delay it forever to stimulate the loading state
        mockHttp.When("*").Respond(async req =>
        {
            await Task.Delay(Timeout.Infinite); // never resolves
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        Services.AddSingleton<HttpClient>(mockHttp.ToHttpClient()); // Wraps it inside a real httpclient instance

        // Act
        var cut = RenderComponent<MovieComponent>();

        // Assert
        cut.FindAll("p").Last().MarkupMatches("<p>Loading...</p>");
    }

        [Fact]
    public async Task MovieComponent_RendersMoviesAfterApiLoad()
    {
        // Arrange
        fakeSearchState.SetQuery("inception");
        var fakeMovies = new List<Movie>
        {
            new Movie{Title="Inception",Overview="Test",ReleaseDate="2026-01-01"},
            new Movie{Title="Inception 2", Overview="Test 2",ReleaseDate="2026-02-02"}
        };
        SetupHttpClient(fakeMovies);

        // Act
        var cut = RenderComponent<MovieComponent>();
        cut.WaitForAssertion(() => Assert.Equal(2, cut.FindAll("h3").Count));

        // Assert – both movie titles appear
        var headings = cut.FindAll("h3");
        Assert.Equal("Inception", headings[0].TextContent);
        Assert.Equal("Inception 2", headings[1].TextContent);
    }
}