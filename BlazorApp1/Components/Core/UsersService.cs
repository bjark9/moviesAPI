using BlazorApp1.Components.Core;

public class UserService
{
    public List<User> Users { get; set; } = new(); // initialize list so Add() won't throw bc of null error

    // scoped service ensures one instance per circuit/session
    public User? CurrentUser { get; private set; }

    public event Action? OnChange;

    // Register a new account
    public bool Register(string username, string password)
    {
        if (Users.Any(u => u.Username == username))
        {
            return false; // Username already taken
        }

        // user constructor hashes password automatically
        Users.Add(new User(username, password));
        // don't automatically log in the new user
        return true;
    }

    // Attempt to log in. On success the "CurrentUser" property is set 
    public bool Login(string username, string password)
    {
        var user = Users.FirstOrDefault(u => u.Username == username);
        if (user is null || !user.VerifyPassword(password, user.HashedPassword))
        {
            return false;
        }

        CurrentUser = user;
        NotifyStateHasChanged();
        return true;
    }

    public void Logout()
    {
        CurrentUser = null;
        NotifyStateHasChanged();
    }

    private void NotifyStateHasChanged()
    {
        OnChange?.Invoke();
    }
}