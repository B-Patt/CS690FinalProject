using PackingListApp.Domain;

namespace PackingListApp.Tests;

public class ValidatorTests
{
    [Fact]
    public void Test_ValidName()
    {
        Assert.True(Validator.IsValidListName("Trip1"));
    }

    [Fact]
    public void Test_Invalid_Empty()
    {
        Assert.False(Validator.IsValidListName(""));
    }

    [Fact]
    public void Test_Invalid_Whitespace()
    {
        Assert.False(Validator.IsValidListName("   "));
    }

    [Fact]
    public void Test_Invalid_Characters()
    {
        Assert.False(Validator.IsValidListName("Bad|Name"));
        Assert.False(Validator.IsValidListName("Bad<Name"));
    }

    [Fact]
    public void Test_Invalid_NoLettersOrDigits()
    {
        Assert.False(Validator.IsValidListName("!!!"));
    }
}
