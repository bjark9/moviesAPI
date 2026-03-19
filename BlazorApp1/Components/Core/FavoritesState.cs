using BlazorApp1.Components.Core;

public class FavoritesState
{
    private readonly UserService _userService;

    public FavoritesState(UserService userService)
    {
        _userService = userService;
        // each time UserService changes -> notify and re-render
        _userService.OnChange += NotifyStateHasChanged;
    }

    public IReadOnlyList<Movie> Favorites =>
        _userService.CurrentUser?.UserFavorites ?? Array.Empty<Movie>();

    public event Action? OnChange;

    public void AddToFavorites(Movie movie)
    {
        var user = _userService.CurrentUser;
        if (user == null)
        {
            return; 
        }

        if (!user.userFavorites.Contains(movie))
        {
            user.userFavorites.Add(movie);
            NotifyStateHasChanged();
        }
    }

    public void RemoveFromFavorites(Movie movie)
    {
        var user = _userService.CurrentUser;
        if (user == null)
        {
            return;
        }

        if (user.userFavorites.Contains(movie))
        {
            user.userFavorites.Remove(movie);
            NotifyStateHasChanged();
        }
    }

    private void NotifyStateHasChanged()
    {
        OnChange?.Invoke();
    }
}