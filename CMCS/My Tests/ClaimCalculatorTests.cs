using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMCS.Helper;
namespace My_Tests;


[TestClass]
public class ClaimCalculatorTests
{
    [TestMethod]
    public void CalculateTotal_ShouldReturnCorrectValue_WhenInputIsValid()
    {
        //Arrange
        var calculator = new ClaimCalculator();

        //act
        var total = calculator.CalculateTotal("5", "100");

        //Assert
        Assert.IsNotNull(total);
        Assert.AreEqual(500, total.Value);
    }
    [TestMethod]
    public void CalculateTotal_ShouldreturnNull_WhenInputIsInvalid()
    {
        //Arrange
        var calculator = new ClaimCalculator();

        //Act
        var total1 = calculator.CalculateTotal("abc", "100");
        var total2 = calculator.CalculateTotal("5", "xyz");

        Assert.IsNull(total1);
        Assert.IsNull(total2);
    }
    
}
