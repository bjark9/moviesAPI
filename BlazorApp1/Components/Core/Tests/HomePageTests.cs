using BlazorApp1.Components.Pages;
using Microsoft.AspNetCore.Components;
using Bunit;
using Xunit;

// Run dotnet test for testing 

public class HomeTests : TestContext
{
    private readonly FakeSearchState _searchState;

    public HomeTests()
    {
        _searchState = new FakeSearchState();
        // Singleton service is fine here (instead of scoped), bUnit's TestContext doesn't understand scopes -> will act the same 
        Services.AddSingleton(_searchState);
    }
    
    [Fact]
    public void Home_RendersSearchInputAndButton()
    {
        // TEST: Renders the search input button

        // Render Component Home.razor
        var cut = RenderComponent<Home>();

        // Assert
        cut.Find("input[type='search']").MarkupMatches("<input type=\"search\" id=\"site-search\" />");
        cut.Find("button").MarkupMatches("<button>Search</button>");
    }

    [Fact]
    public void Home_RendersHeaderCorrectly()
    {
        // TEST : Renders the Header

        // Render component Home.razor
        var cut = RenderComponent<Home>();

        // Assert
        cut.Find("h1").MarkupMatches("<h1>Welcome: Millions of movies and TV shows to discover.</h1>");
    }

    [Fact]
    public void Home_SavesInputAndNavigateToMovieComponent()
    {
        // TEST: Checks if search input gets saved and if navigation to /MovieComponent happens

        var cut = RenderComponent<Home>();
        var input = cut.Find("input[type='search']");

        // Act, create input and search it
        input.Change("Star Wars");
        cut.Find("button").Click();

        // Assert, SearchState should hold the input
        Assert.Equal("Star Wars", _searchState.Query.ToLower());

        // Assert, navigationmanager should have navigated to /MovieComponent
        var navManager = Services.GetRequiredService<NavigationManager>();
        Assert.EndsWith("/MovieComponent",navManager.Uri);
    }

    [Fact]
    public void Home_EmptySearch_DoesNotCrash()
    {
        // TEST: Test if an empty search does not crash the page

        var cut = RenderComponent<Home>();

        // Act – click Search without typing anything
        cut.Find("button").Click();

        // Assert – query stays empty, no exception thrown
        Assert.Equal(string.Empty, _searchState.Query);
    }
}
    