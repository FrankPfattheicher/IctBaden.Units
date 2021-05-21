using Xunit;

namespace IctBaden.Units.Test.PhoneNumbers
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

        [Fact]
        public void FrenchNumberWithPlusShouldBeParsed()
        {
            const string number = "+33636854544";
            
            var phone = PhoneNumber.TryParse(number);

            Assert.True(!phone.IsEmpty);
            Assert.True(phone.IsValid);
            Assert.Equal("33", phone.CountryCode);
        }

        [Fact]
        public void UkNumberWithPlusShouldBeParsed()
        {
            const string number = "+441865824000";
            
            var phone = PhoneNumber.TryParse(number);

            Assert.True(!phone.IsEmpty);
            Assert.True(phone.IsValid);
            Assert.Equal("44", phone.CountryCode);
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