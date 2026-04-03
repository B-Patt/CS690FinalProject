namespace PackingListApp;

public static class Validator
{
    public static bool IsValidListName(string name)
    {
        return !string.IsNullOrWhiteSpace(name);
    }
}