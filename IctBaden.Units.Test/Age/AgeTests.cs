using System;
using Xunit;

namespace IctBaden.Units.Test.Age
{
    public class AgeTests
    {
        [Fact]
        public void AgeShouldBeZeroAtBirthdate()
        {
            var age = new Units.Age(DateTime.Parse("2.10.1959"), DateTime.Parse("2.10.1959"));
            
            Assert.Equal(0, age.Years);
            Assert.Equal(0, age.Months);
            Assert.Equal(0, age.Days);
        }
        
        [Fact]
        public void AgeDaysAndMonthsShouldBeZeroAtBirthday()
        {
            var age = new Units.Age(DateTime.Parse("2.10.1959"), DateTime.Parse("2.10.2059"));
            
            Assert.Equal(100, age.Years);
            Assert.Equal(0, age.Months);
            Assert.Equal(0, age.Days);
        }
        
        [Fact]
        public void AgeShouldBeCorrect()
        {
            var age = new Units.Age(DateTime.Parse("2.10.1959"), DateTime.Parse("6.12.2021"));
            
            Assert.Equal(62, age.Years);
            Assert.Equal(2, age.Months);
            Assert.Equal(4, age.Days);
        }
        
    }
}