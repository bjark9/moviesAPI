public class SearchState
{
    public string Query {get; private set;}
    public event Action? OnChange;
    public void SetQuery(string query)
    {
        Query = query;
        OnChange?.Invoke(); // Notify components of the change.
    }

}