namespace PackingListApp.Interfaces;

public interface IStorage
{
    string ReadFile(string name);
    void WriteFile(string name, string contents);
    void RenameFile(string oldName, string newName);
    void DeleteFile(string name);
    bool FileExists(string name);
    List<string> ListFiles();
}