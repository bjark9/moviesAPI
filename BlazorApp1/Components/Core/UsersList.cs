using BlazorApp1.Components.Core;

public class UsersList
{
    public List<User> Users {get;set;} = new(); // initialize so no Null error when adding users to list
}