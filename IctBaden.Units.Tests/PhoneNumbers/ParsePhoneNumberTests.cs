using Xunit;

namespace IctBaden.Units.Tests.PhoneNumbers
{
    public class ParsePhoneNumberTests
    {
        [Fact]
        public void InternationalNumberWithPlusShouldBeParsed()
        {
            const string number = "+4972140989106";
            
            var phone = new PhoneNumber(number);

            Assert.True(!phone.IsEmpty);
            Assert.Equal("721", phone.AreaCode);
            Assert.Equal("49", phone.CountryCode);
        }
        
        [Fact]
        public void InternationalNumberWithoutPlusShouldBeParsed()
        {
            const string number = "49 721-40989106";
            
            var phone = new PhoneNumber(number);

            Assert.True(!phone.IsEmpty);
            Assert.Equal("721", phone.AreaCode);
            Assert.Equal("49", phone.CountryCode);
        }
        
        [Fact]
        public void NumberWithLeadingZerosShouldBeParsed()
        {
            const string number = "004972140989106";
            
            var phone = new PhoneNumber(number);

            Assert.True(!phone.IsEmpty);
            Assert.Equal("721", phone.AreaCode);
            Assert.Equal("49", phone.CountryCode);
        }
        
        [Fact]
        public void FormatInternationalNumberShouldBeWithPlus()
        {
            const string expected = "+4972140989106";
            
            const string number1 = "+4972140989106";
            var phone = new PhoneNumber(number1);
            var text = phone.GetDialString("+EFG");
            Assert.Equal(expected, text);

            const string number2 = "4972140989106";
            phone = new PhoneNumber(number2);
            text = phone.GetDialString("+EFG");
            Assert.Equal(expected, text);

            const string number3 = "072140989106";
            phone = new PhoneNumber(number3);
            text = phone.GetDialString("+EFG");
            Assert.Equal(expected, text);
            
        }
        
        [Fact]
        public void FormatMobileShouldBeWithPlus()
        {
            const string expected = "+491727207196";
            
            const string number1 = "+491727207196";
            var phone = new PhoneNumber(number1);
            var text = phone.GetDialString("+EFG");
            Assert.Equal(expected, text);

            const string number2 = "491727207196";
            phone = new PhoneNumber(number2);
            text = phone.GetDialString("+EFG");
            Assert.Equal(expected, text);

            const string number3 = "01727207196";
            phone = new PhoneNumber(number3);
            text = phone.GetDialString("+EFG");
            Assert.Equal(expected, text);
            
        }
        
    }
}