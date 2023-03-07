using System;
using System.Globalization;
using Xunit;

namespace IctBaden.Units.Test.Age;

public class AgeTests
{
    private readonly CultureInfo _culture;

    public AgeTests()
    {
        _culture = new CultureInfo("de-DE");
    }
        
    [Fact]
    public void AgeShouldBeZeroAtBirthdate()
    {
        var age = new Units.Age(DateTime.Parse("2.10.1959", _culture), DateTime.Parse("2.10.1959", _culture));
            
        Assert.Equal(0, age.Years);
        Assert.Equal(0, age.Months);
        Assert.Equal(0, age.Days);
    }
        
    [Fact]
    public void AgeDaysAndMonthsShouldBeZeroAtBirthday()
    {
        var age = new Units.Age(DateTime.Parse("2.10.1959", _culture), DateTime.Parse("2.10.2059", _culture));
            
        Assert.Equal(100, age.Years);
        Assert.Equal(0, age.Months);
        Assert.Equal(0, age.Days);
    }
        
    [Fact]
    public void AgeShouldBeCorrect()
    {
        var age = new Units.Age(DateTime.Parse("2.10.1959", _culture), DateTime.Parse("6.12.2021", _culture));
            
        Assert.Equal(62, age.Years);
        Assert.Equal(2, age.Months);
        Assert.Equal(4, age.Days);
    }
        
}