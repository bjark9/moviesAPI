// Replaces the real one so the testing stays isolated from the app state.
public class FakeSearchState
{
    public string Query {get;private set;} = string.Empty;
    public event Action? OnChange;
    public void SetQuery(string query)
    {
        Query = query;
        OnChange?.Invoke();
    }

}