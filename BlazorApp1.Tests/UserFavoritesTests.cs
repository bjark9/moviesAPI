using Xunit;
using BlazorApp1.Components.Core;

public class UserFavoritesTests
{
    [Fact]
    public void DifferentUsersHaveSeparateFavorites()
    {
        // set up services 
        var userService = new FakeUserService();
        var favState = new FakeFavoritesState(userService);

        // create two accounts
        Assert.True(userService.Register("user1", "1234"));
        Assert.True(userService.Register("user2", "12345"));

        // user1 adds movie to favorites
        Assert.True(userService.Login("user1", "1234"));
        var movieA = new Movie { Title = "A", Overview = "x", ReleaseDate = "2026-01-01" };
        favState.AddToFavorites(movieA);
        Assert.Contains(movieA, favState.Favorites);

        // switch to user2 and verify user1 favorite isn't present
        Assert.True(userService.Login("user2", "12345"));
        Assert.Empty(favState.Favorites);

        var movieB = new Movie { Title = "B", Overview = "y", ReleaseDate = "2026-02-02" };
        favState.AddToFavorites(movieB);
        Assert.Contains(movieB, favState.Favorites);

        // switch back to user1
        Assert.True(userService.Login("user1", "1234"));
        Assert.Contains(movieA, favState.Favorites);
        Assert.DoesNotContain(movieB, favState.Favorites);
    }
}