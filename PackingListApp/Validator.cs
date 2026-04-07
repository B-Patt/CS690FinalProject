namespace PackingListApp;

public static class Validator
{
    public static bool IsValidlistName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }
        
        char[] invalidChars = { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

        for (int i = 0; i < invalidChars.Length; i++)
        {
            if (name.Contains(invalidChars[i]))
            {
                return false;
            }
        }
        
        bool hasLetterOrDigit = false;

        for (int i = 0; i < name.Length; i++)
        {
            if (char.IsLetterOrDigit(name[i]))
            {
                hasLetterOrDigit = true;
                break;
            }
        }

        if (!hasLetterOrDigit)
        {
            return false;
        }

        return true;
    }

}