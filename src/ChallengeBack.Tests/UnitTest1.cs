namespace ChallengeBack.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Assert.True(true);
    }

    [Fact]
    public void BasicMathTest()
    {
        int result = 2 + 2;
        Assert.Equal(4, result);
    }

    [Fact]
    public void StringTest()
    {
        string message = "Hello, World!";
        Assert.NotEmpty(message);
        Assert.Contains("World", message);
    }
}
