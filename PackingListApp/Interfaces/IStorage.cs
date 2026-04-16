using System.Collections.Generic;

namespace PackingListApp.Interfaces;

public interface IStorage
{
    string ReadFile(string name);
    void WriteFile(string name, string contents);
    void DeleteFile(string name);
    bool RenameFile(string oldName, string newName);
    List<string> ListFiles();
}