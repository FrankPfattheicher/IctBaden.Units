using Xunit;

namespace IctBaden.Units.Tests.PhoneNumbers
{
    public class ParsePhoneNumberTests
    {
        [Fact]
        public void NumberWithDelimitersShouldBeParsed()
        {
            const string number = "+49 721-40989106";
            
            var phone = PhoneNumber.TryParse(number);

            Assert.True(!phone.IsEmpty);
            Assert.Equal("721", phone.AreaCode);
            Assert.Equal("49", phone.CountryCode);
        }
        
        [Fact]
        public void InternationalNumberWithPlusShouldBeParsed()
        {
            const string number = "+4972140989106";
            
            var phone = PhoneNumber.TryParse(number);

            Assert.True(!phone.IsEmpty);
            Assert.Equal("721", phone.AreaCode);
            Assert.Equal("49", phone.CountryCode);
        }
        
        [Fact]
        public void InternationalNumberWithoutPlusShouldBeParsed()
        {
            const string number = "4972140989106";

            var phone = PhoneNumber.TryParse(number);

            Assert.True(!phone.IsEmpty);
            Assert.Equal("721", phone.AreaCode);
            Assert.Equal("49", phone.CountryCode);
        }
        
        [Fact]
        public void NumberWithLeadingZerosShouldBeParsed()
        {
            const string number = "004972140989106";
            
            var phone = PhoneNumber.TryParse(number);

            Assert.True(!phone.IsEmpty);
            Assert.Equal("721", phone.AreaCode);
            Assert.Equal("49", phone.CountryCode);
        }
        
        // [Fact]
        // public void NumberWithLeadingZerosButWithoutCountryCodeShouldBeParsed()
        // {
        //     const string number = "0072140989106";
        //     
        //     var phone = PhoneNumber.TryParse(number);
        //
        //     Assert.True(!phone.IsEmpty);
        //     Assert.Equal("721", phone.AreaCode);
        //     Assert.Equal("49", phone.CountryCode);
        // }
        
    }
}