namespace PackingListApp.Tests;

public class TextFileStorageTests
{
    private readonly string testDir = "TestLists";
    private TextFileStorage storage;

    public TextFileStorageTests()
    {
        if (Directory.Exists(testDir))
            Directory.Delete(testDir, true);

        storage = new TextFileStorage(testDir);
    }

    [Fact]
    public void Test_WriteAndReadFile()
    {
        storage.WriteFile("test.txt", "hello");
        string contents = storage.ReadFile("test.txt");

        Assert.Equal("hello", contents);
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
        storage.WriteFile("a.txt", "1");
        storage.WriteFile("b.txt", "2");

        var files = storage.ListFiles();

        Assert.Contains("a.txt", files);
        Assert.Contains("b.txt", files);
    }
}
