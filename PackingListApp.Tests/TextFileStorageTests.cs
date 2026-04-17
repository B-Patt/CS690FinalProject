using PackingListApp.Infrastructure;

namespace PackingListApp.Tests;

public class TextFileStorageTests
{
    private readonly string testDir;
    private TextFileStorage storage;

    public TextFileStorageTests()
    {
        testDir = Path.Combine(Path.GetTempPath(), "PackingListAppTests_Storage");

        if (Directory.Exists(testDir))
            Directory.Delete(testDir, true);

        Directory.CreateDirectory(testDir);

        storage = new TextFileStorage(testDir);
    }

    [Fact]
    public void Test_WriteAndReadFile()
    {
        storage.WriteFile("test.txt", "toothbrush");
        string contents = storage.ReadFile("test.txt");

        Assert.Equal("toothbrush", contents);
    }

    [Fact]
    public void Test_FileExists()
    {
        storage.WriteFile("exists.txt", "data");
        Assert.True(storage.FileExists("exists.txt"));
    }

    [Fact]
    public void Test_DeleteFile()
    {
        storage.WriteFile("delete.txt", "x");
        storage.DeleteFile("delete.txt");

        Assert.False(storage.FileExists("delete.txt"));
    }

    [Fact]
    public void Test_ListFiles()
    {
        storage.WriteFile("testfile1.txt", "1");
        storage.WriteFile("testfile2.txt", "2");

        var files = storage.ListFiles();

        Assert.Contains("testfile1", files);
        Assert.Contains("testfile2", files);
    }
}
