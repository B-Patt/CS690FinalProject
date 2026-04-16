using PackingListApp.Interfaces;

namespace PackingListApp.Infrastructure
{
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

        private string GetFullPath(string name)
        {
            // Always ensure .txt extension
            if (!name.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                name += ".txt";

            return Path.Combine(directoryPath, name);
        }

        public string ReadFile(string name)
        {
            string fullPath = GetFullPath(name);

            if (!File.Exists(fullPath))
                return "";

            return File.ReadAllText(fullPath);
        }

        public void WriteFile(string name, string contents)
        {
            string fullPath = GetFullPath(name);
            File.WriteAllText(fullPath, contents);
        }

        public bool RenameFile(string oldName, string newName)
        {
            string oldPath = GetFullPath(oldName);
            string newPath = GetFullPath(newName);

            if (!File.Exists(oldPath))
                return false;

            File.Move(oldPath, newPath);
            return true;
        }

        public void DeleteFile(string name)
        {
            string fullPath = GetFullPath(name);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public bool FileExists(string name)
        {
            string fullPath = GetFullPath(name);
            return File.Exists(fullPath);
        }

        public List<string> ListFiles()
        {
            return Directory.GetFiles(directoryPath, "*.txt")
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }
    }
}
