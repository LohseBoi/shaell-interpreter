using Xunit;

namespace ShaellLang.Test;

public class UnitTest1
{
    [Fact]
    public void TestOperators()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();
        
        shaellLang.RunFile("../../../OperatorTest.æ");
    }
    
    [Fact]
    public void TestMetaTables()
    {   
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();

        shaellLang.RunFile("../../../MetatableTest.æ");
    }

    [Fact]
    public void TestStringInterpolation()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();

        shaellLang.RunFile("../../../StringInterpolationTest.æ");
    }
    
    [Fact]
    public void TestScope()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();

        shaellLang.RunFile("../../../ScopeTest.æ");
    }

    [Fact]
    public void TestForeach()
    {
        // Act
        bool testFailed = shaellLang.Run("../../../ForeachTest.æ", new string[]{"ForeachTest.æ", ""});
        
        // Assert
        Assert.False(testFailed);
    }
    
    [Fact]
    public void TestTryThrow()
    {
        ShaellLang shaellLang = new ShaellLang();
        shaellLang.LoadStdLib();

        shaellLang.RunFile("../../../TryThrowTest.æ");
    }
}