using Xunit;

namespace ShaellLang.Test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        ShaellLang shaellLang = new ShaellLang();
        
        // Act  
        bool testFailed = shaellLang.Run("../../../test.æ");

        // Assert
        Assert.False(testFailed);
    }
}