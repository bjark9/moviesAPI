using BlazorApp1.Components.Core;

public class FavoritesState
{
    private List<Movie> favorites = new(); // Only accessible from inside FavoritesState
    public IReadOnlyList<Movie> Favorites => favorites; // Accessible from everywhere in Readonly 
    public event Action? OnChange;
    public void AddToFavorites(Movie movie)
    {
        if (!favorites.Contains(movie))
        {
            favorites.Add(movie);
            NotifyStateHasChanged();
        }
    }

    public void RemoveFromFavorites(Movie movie)
    {
        if (favorites.Contains(movie))
        {
            favorites.Remove(movie);
            NotifyStateHasChanged();
        }
    }
    private void NotifyStateHasChanged()
    {
        OnChange?.Invoke();
    }
}