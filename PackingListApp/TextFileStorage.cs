namespace PackingListApp.Infrastructure;

using PackingListApp.Interfaces;

public class TextFileStorage : IStorage
{
    private readonly string directoryPath;

    public TextFileStorage(string directoryPath)
    {
        this.directoryPath = directoryPath;

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }

    public string ReadFile(string name)
    {
        string fullPath = Path.Combine(directoryPath, name);

        if (!File.Exists(fullPath))
        {
            return "";
        }

        return File.ReadAllText(fullPath);
    }

    public void WriteFile(string name, string contents)
    {
        string fullPath = Path.Combine(directoryPath, name);
        File.WriteAllText(fullPath, contents);
    }

    public void RenameFile(string oldName, string newName)
    {
        string oldPath = Path.Combine(directoryPath, oldName);
        string newPath = Path.Combine(directoryPath, newName);

        if (File.Exists(oldPath))
        {
            File.Move(oldPath, newPath);
        }
    }

    public void DeleteFile(string name)
    {
        string fullPath = Path.Combine(directoryPath, name);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public bool FileExists(string name)
    {
        string fullPath = Path.Combine(directoryPath, name);
        return File.Exists(fullPath);
    }

    public List<string> ListFiles()
    {
        List<string> files = new List<string>();

        string[] filePaths = Directory.GetFiles(directoryPath);

        foreach (string path in filePaths)
        {
            files.Add(Path.GetFileName(path));
        }

        return files;
    }
}
